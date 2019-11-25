namespace Searchfight.Service
{
    public interface ISearch
    {
        long SearchResult { get; set; }

        long Search(string keyword);

        void ProcessSearchResult(string json);

        long Winner { get; set; }
        string WinnerKeyword { get; set; }
    }
}
