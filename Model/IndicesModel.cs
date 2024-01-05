namespace FluentSearchEngine.Model
{
    public class IndicesModel : SearchModel<string>
    {
        public IndicesModel(string id, string data, string index)
        {
            Id = id;
            Data = data;
            Index = index;
        }

        public string Data { get; set; }

        public string Index { get; set; }
    }
}
