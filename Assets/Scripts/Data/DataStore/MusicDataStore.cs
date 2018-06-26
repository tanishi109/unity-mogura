using CAFU.Core.Data.DataStore;
using CAFU.Music.Data.DataStore;
using Mogura.Data.Entity;
using Mogura.Constants;

namespace Mogura.Data.DataStore
{
    public interface IMusicDataStore : IDataStore
    {
    }

    public class MusicDataStore : MusicDataStoreSingle<MusicName, MusicEntity> {}
}