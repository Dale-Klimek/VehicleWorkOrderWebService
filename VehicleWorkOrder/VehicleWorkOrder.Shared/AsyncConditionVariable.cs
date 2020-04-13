namespace VehicleWorkOrder.Shared
{
    using System.Threading;
    using System.Threading.Tasks;

    public class AsyncConditionVariable
    {
        private readonly SemaphoreSlim _mutex = new SemaphoreSlim(0);

        public Task WaitAsync()
        {
            return _mutex.WaitAsync();
        }

        public void Notify(int value = 1)
        {
            _mutex.Release(value);
        }
    }
}
