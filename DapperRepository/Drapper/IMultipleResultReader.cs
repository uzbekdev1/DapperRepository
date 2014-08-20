using System.Collections.Generic;

namespace DapperRepository.Drapper
{
    public interface IMultipleResultReader
    {
        IEnumerable<T> Read<T>();
    }
}