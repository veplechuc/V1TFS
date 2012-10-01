using VersionOne.TFS2010.DataLayer;

namespace VersionOne.TFS.Policy {
    public interface IV1ComponentContainerProvider {
        V1ComponentContainer GetContainer();
    }
}