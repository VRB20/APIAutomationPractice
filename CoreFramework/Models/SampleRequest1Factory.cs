using CoreFramework.ObjectGraphBatchValidation.Models;
using System;
using System.Linq;

namespace CoreFramework.Models
{
    public class SampleRequest1Factory
    {
        public static SampleRequest1 CreateRequest(Batch batch)
        {
            if (!batch.Any()) throw new ArgumentNullException(nameof(batch));

            return new SampleRequest1
            {
                Name = batch[0].GetValueOrDefault("name"),
                Job = batch[0].GetValueOrDefault("job"),
                Email = batch[0].GetValueOrDefault("email"),
                Password = batch[0].GetValueOrDefault("password")
            };
        }
    }
}
