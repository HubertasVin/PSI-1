namespace Project.Models
{
    public class Conspect : BaseModel
    {
        private string? _title;
        private string? _authorId;
        private string? _subjectId;
        private string? _topicId;
        private string? _fileName;
        private int? _index;
 
        public string? Title
        {
            get => _title;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Title cannot be null");
                }
                if (value.Length < 3)
                {
                    throw new ArgumentException("Title must be at least 3 characters long");
                }
                _title = value;
            }
        }
 
        public string? AuthorId
        {
            get => _authorId;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("AuthorId cannot be null");
                }
                if (value.Length < 3)
                {
                    throw new ArgumentException("AuthorId must be at least 3 characters long");
                }
                _authorId = value;
            }
        }

        public string? SubjectId
        {
            get => _subjectId;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("SubjectId cannot be null");
                }
                if (value.Length < 3)
                {
                    throw new ArgumentException("SubjectId must be at least 3 characters long");
                }
                _subjectId = value;
            }
        }

        public string? TopicId
        {
            get => _topicId;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("TopicId cannot be null");
                }
                if (value.Length < 3)
                {
                    throw new ArgumentException("TopicId must be at least 3 characters long");
                }
                _topicId = value;
            }
        }
 
        public string? FileName
        {
            get => _fileName;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("FileName cannot be null");
                }
                if (value.Length < 3)
                {
                    throw new ArgumentException("FileName must be at least 3 characters long");
                }
                _fileName = value;
            }
        }
 
        public int? Index
        {
            get => _index;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Index cannot be null");
                }
                if (value < 0)
                {
                    throw new ArgumentException("Index must be at least 0");
                }
                _index = value;
            }
        }
 
        public string? ConspectLocation
        {
            get => "uploads/" + _subjectId + "/" + _topicId + "/" + _fileName;
        }
    }
}