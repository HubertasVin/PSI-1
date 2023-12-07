using System.Text.Json;
using System.Text.RegularExpressions;
using Project.Data;
using Project.Exceptions;
using Project.Models;
 
namespace Project.Repository;
 
public class ConspectRepository : Repository<Conspect>, IConspectRepository
{
    public NoteBlendDbContext? NoteBlendContext => Context as NoteBlendDbContext;
 
    public ConspectRepository(NoteBlendDbContext context) : base(context)
    {
 
    }
 
    public List<Conspect> GetConspectList()
    {
        return NoteBlendContext?.Conspects.ToList() ?? new List<Conspect>();
    }
 
    public List<Conspect> GetConspectListByTopicId(string topicId)
    {
        return NoteBlendContext?.Conspects.Where(conspect => conspect.TopicId == topicId).ToList() ?? new List<Conspect>();
    }
 
    public Conspect? GetConspectByTopicIdAndIndex(string topicId, int index)
    {
        Conspect? conspect = NoteBlendContext?.Conspects.FirstOrDefault(conspect => conspect.TopicId == topicId && conspect.Index == index);
        return conspect ?? null;
    }
 
    public async Task<Conspect?> CreateConspect(Conspect newConspect, IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            newConspect.Index = NoteBlendContext?.Conspects.Where(conspect => conspect.TopicId == newConspect.TopicId).Count() ?? 0;
            Directory.CreateDirectory("ClientApp/public/uploads/" + newConspect.SubjectId + "/" + newConspect.TopicId);
            if (DoesFileExist("ClientApp/" + newConspect.ConspectLocation))
            {
                throw new ConspectAlreadyExistsException("Conspect already exists");
            }
            Add(newConspect);
            int changes = NoteBlendContext.SaveChanges();
            using (Stream FileStream = new FileStream("ClientApp/public/" + newConspect.ConspectLocation, FileMode.Create))
            {
                await file.CopyToAsync(FileStream);
            }
            return changes > 0 ? newConspect : null;
        }
        return null;
    }

    public void DeleteConspect(string id, int index, string authorId)
    {
        Conspect? conspect = GetConspectByTopicIdAndIndex(id, index);
        if (conspect == null)
        {
            throw new ConspectNotFoundException("Conspect not found");
        }
        if (conspect.AuthorId != authorId)
        {
            throw new ConspectNotDeletedException("You are not the original author of the conspect");
        }
        NoteBlendContext?.Conspects.Remove(conspect);
        NoteBlendContext?.SaveChanges();
        if (DoesFileExist("ClientApp/" + conspect.ConspectLocation))
        {
            File.Delete("ClientApp/" + conspect.ConspectLocation);
        }
    }

    public bool DoesFileExist(string conspectLocation)
    {
        return File.Exists(conspectLocation);
    }
}