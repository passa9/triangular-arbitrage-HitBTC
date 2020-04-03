using System;
using System.Threading;
using System.Threading.Tasks;
using HitBTC.Net.Communication;
using HitBTC.Net.Models;
using Newtonsoft.Json.Linq;

namespace HitBTC.Net.Utils
{
    internal class HitResponseWaiter<T> : IHitResponseWaiter
    {
        private HitResponse<T> response;
        private Exception parseException;
        private bool aborted;

        private readonly CancellationTokenSource cancellationTokenSource;

        public HitResponseWaiter(TimeSpan timeout) => this.cancellationTokenSource = new CancellationTokenSource(timeout);

        public async Task WaitAsync(CancellationToken externalToken)
        {
            try
            {
                while (!this.cancellationTokenSource.IsCancellationRequested && !externalToken.IsCancellationRequested)
                    await Task.Delay(100);
            }
            catch (TaskCanceledException tce)
            { }
        }

        public void Abort()
        {
            this.aborted = true;
            this.cancellationTokenSource.Cancel();
        }

        public void TryParseResponse(JObject jObject)
        {
            try
            {
                this.response = jObject.ToObject<HitResponse<T>>();
            }
            catch (Exception ex)
            {
                this.parseException = ex;
            }
            finally
            {
                this.Abort();
            }
        }

        public HitResponse<TResult> GetResponse<TResult>()
        {
            if (this.parseException != null)
                return new HitResponse<TResult>
                {
                    Error = new HitError(HitError.CODE_RESPONSE_PARSE_ERROR, this.parseException.InnerException?.Message ?? this.parseException.Message)
                };

            if (this.response == null)
            {
                if (this.aborted)
                    return new HitResponse<TResult>
                    {
                        Error = new HitError(HitError.CODE_REQUEST_ABORTED, "Request was aborted")
                    };
                else
                    return new HitResponse<TResult>
                    {
                        Error = new HitError(HitError.CODE_REQUEST_TIMEOUT, "Request aborted by timeout")
                    };
            }


            if (this.response is HitResponse<TResult> result)
                return result;
            else
                return new HitResponse<TResult>
                {
                    Error = new HitError(HitError.CODE_REQUEST_ABORTED, "Request was aborted")
                };
        }

        public void SetResponse(HitResponse response)
        {
            this.response = response as HitResponse<T>;

            this.Abort();
        }
    }

    internal interface IHitResponseWaiter
    {
        Task WaitAsync(CancellationToken externalToken);
        
        void TryParseResponse(JObject jObject);

        HitResponse<TResult> GetResponse<TResult>();
    }
}