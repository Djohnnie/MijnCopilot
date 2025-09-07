namespace MijnCopilot.Web.Helpers;

public class NavigationHelper
{
    private List<NavigationItem> _links = new List<NavigationItem>();
    public IEnumerable<NavigationItem> Links => _links.AsEnumerable();

    public event EventHandler? LinksChanged;

    public void AddLink(Guid id, string title, string url, bool insert = false)
    {
        if (!_links.Any(item => item.Title == title))
        {
            if (insert)
            {
                _links.Insert(0, new NavigationItem(id, title, url));
            }
            else
            {
                _links.Add(new NavigationItem(id, title, url));
            }
            LinksChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void RemoveLink(Guid chatId)
    {
        var item = _links.FirstOrDefault(item => item.Id == chatId);
        if (item != null)
        {
            _links.Remove(item);
            LinksChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

public record NavigationItem(Guid Id, string Title, string Url);