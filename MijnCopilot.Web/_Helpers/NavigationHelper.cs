namespace MijnCopilot.Web.Helpers;

public class NavigationHelper
{
    private List<NavigationItem> _links = new List<NavigationItem>();
    public IEnumerable<NavigationItem> Links => _links.AsEnumerable();

    public event EventHandler? LinksChanged;

    public void AddLink(string title, string url, bool insert = false)
    {
        if (!_links.Any(item => item.Title == title))
        {
            if (insert)
            {
                _links.Insert(0, new NavigationItem(title, url));
            }
            else
            {
                _links.Add(new NavigationItem(title, url));
            }
            LinksChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

public record NavigationItem(string Title, string Url);