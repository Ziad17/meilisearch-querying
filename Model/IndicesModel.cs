namespace FluentSearchEngine.Model
{
    public class IndicesModel<T> : SearchModel<T>
    {
        public IndicesModel(T id, string data, string index)
        {
            Id = id;
            Data = data;
            Index = index;
        }

        public string Data { get; set; }

        public string Index { get; set; }
    }
}
