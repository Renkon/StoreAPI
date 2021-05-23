using System;

namespace StoreAPI.Core.Exceptions
{
    public class DocumentNotFoundException<T> : Exception
    {
        public DocumentNotFoundException(T id) : base($"Element with id {id} was not found")
        {
            this.Id = id;
        }

        public T Id { get; set; }
    }
}
