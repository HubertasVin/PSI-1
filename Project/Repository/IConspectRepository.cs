using Project.Models;

namespace Project.Repository;

public interface IConspectRepository
{
    public List<Conspect> GetConspectList();
    public List<Conspect> GetConspectListByTopicId(string topicId);
    public Conspect? GetConspectByTopicIdAndIndex(string topicId, int index);
    public Task<Conspect?> CreateConspect(Conspect newConspect, IFormFile file);
    public bool DoesFileExist(string conspectLocation);
}