using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VehicleWorkOrder.Database;
using VehicleWorkOrder.Database.Models;
using VehicleWorkOrder.MobileAppService.Interfaces;
using VehicleWorkOrder.Shared.Models;

namespace VehicleWorkOrder.MobileAppService.Services
{
    public class PhotoService : IPhotoService
    {
        private IBlobStorageService _blobService;
        private readonly WorkOrderContext _context;
        private readonly IMapper _mapper;
        private ImageCodecInfo _codec;
        private EncoderParameters _encoderParameters = new EncoderParameters(1);

        public PhotoService(IBlobStorageService blobService, WorkOrderContext context, IMapper mapper)
        {
            _blobService = blobService;
            _context = context;
            _mapper = mapper;
            _codec = GetEncoder(ImageFormat.Jpeg);
            var qualityEncoder = Encoder.Quality;
            _encoderParameters.Param[0] = new EncoderParameter(qualityEncoder, 25L);
        }

        public async Task<PhotoDto> GetPhoto(long id, bool compress = true)
        {
            var photo = await _context.Photos.SingleOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);
            if (photo is null)
                return null;
            var result = _mapper.Map<PhotoDto>(photo);
            result.Photo = (await GetPhoto(result.Name, compress).ConfigureAwait(false)).ToArray();
            return result;
        }

        public async Task<MemoryStream> GetPhoto(Guid name, bool isCompressed = true)
        {

            using var stream = await _blobService.Download(name.ToString());
            var ms = new MemoryStream();
            await stream.CopyToAsync(ms).ConfigureAwait(false);
            if (!isCompressed)
                return ms;
            var result = ConvertImage(ms);
            // dispose of our old memory stream
            await ms.DisposeAsync();
            return result;

        }

        public async Task<Guid> Upload(byte[] photo)
        {
            using var stream = new MemoryStream(photo);
            return await _blobService.Upload(stream).ConfigureAwait(false);
        }

        public async IAsyncEnumerable<PhotoDto> GetPhotosByCar(int carId)
        {
            await foreach (var photoStream in GetPhotos(carId).ConfigureAwait(false))
            {
                await using var download = await GetPhoto(photoStream.Name).ConfigureAwait(false);
                var photo = (PhotoDto)photoStream.Clone();
                photo.Photo = download.ToArray();
                yield return photo;
            }
        }

        public async Task<ICollection<Photo>> UploadPictures(IEnumerable<PhotoDto> pics)
        {
            var list = new List<Photo>();
            foreach (var pic in pics)
            {
                var result = await Upload(pic.Photo).ConfigureAwait(false);
                list.Add(new Photo() { Name = result });
            }
            return list;
        }

        private async IAsyncEnumerable<PhotoDto> GetPhotos(int carId)
        {
            await foreach (var photo in _context.Photos.Where(p => p.CarId == carId && !p.IsDeleted).AsAsyncEnumerable())
            {
                yield return _mapper.Map<PhotoDto>(photo);
            }
        }

        public async Task UpdateDeletedPhotos(IEnumerable<PhotoDto> photos)
        {
            var ids = (from dto in photos where dto.Id.HasValue select dto.Id.Value).ToList();

            await foreach (var photo in _context.Photos.Where(p => ids.Contains(p.Id)).AsAsyncEnumerable())
            {
                photo.IsDeleted = true;
            }
        }

        public async Task<ICollection<PhotoDto>> LoadPhotos(int id)
        {
            var list = new List<PhotoDto>();
            await foreach (var photo in GetPhotosByWorkOrder(id).ConfigureAwait(false))
            {
                list.Add(photo);
            }
            return list;
        }

        public async IAsyncEnumerable<PhotoDto> GetPhotosByWorkOrder(int workOrder)
        {
            await foreach (var photo in _context.Photos.Where(p => p.WorkOrderId == workOrder && !p.IsDeleted)
                .AsAsyncEnumerable().ConfigureAwait(false))
            {
                var picture = _mapper.Map<PhotoDto>(photo);
                await using var download = await GetPhoto(photo.Name).ConfigureAwait(false);
                picture.Photo = download.ToArray();
                yield return picture;
            }
        }

        private MemoryStream ConvertImage(MemoryStream input)
        {
            var stream = new MemoryStream();

            using var image = Image.FromStream(input);

            if (_codec == null)
                image.Save(stream, ImageFormat.Jpeg);
            else
                image.Save(stream, _codec, _encoderParameters);

            return stream;
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            foreach (var codec in ImageCodecInfo.GetImageDecoders())
            {
                if (codec.FormatID == format.Guid)
                    return codec;
            }
            return null;
        }

        void CreateThumbnail(int ThumbnailMax, string OriginalImagePath, string ThumbnailImagePath)
        {
            // Loads original image from file
            Image imgOriginal = Image.FromFile(OriginalImagePath);
            // Finds height and width of original image
            float OriginalHeight = imgOriginal.Height;
            float OriginalWidth = imgOriginal.Width;
            // Finds height and width of resized image
            int ThumbnailWidth;
            int ThumbnailHeight;
            if (OriginalHeight > OriginalWidth)
            {
                ThumbnailHeight = ThumbnailMax;
                ThumbnailWidth = (int)((OriginalWidth / OriginalHeight) * (float)ThumbnailMax);
            }
            else
            {
                ThumbnailWidth = ThumbnailMax;
                ThumbnailHeight = (int)((OriginalHeight / OriginalWidth) * (float)ThumbnailMax);
            }
            // Create new bitmap that will be used for thumbnail
            Bitmap ThumbnailBitmap = new Bitmap(ThumbnailWidth, ThumbnailHeight);
            Graphics ResizedImage = Graphics.FromImage(ThumbnailBitmap);
            // Resized image will have best possible quality
            ResizedImage.InterpolationMode = InterpolationMode.HighQualityBicubic;
            ResizedImage.CompositingQuality = CompositingQuality.HighQuality;
            ResizedImage.SmoothingMode = SmoothingMode.HighQuality;
            // Draw resized image
            ResizedImage.DrawImage(imgOriginal, 0, 0, ThumbnailWidth, ThumbnailHeight);
            // Save thumbnail to file
            ThumbnailBitmap.Save(ThumbnailImagePath);
        }
    }
}
