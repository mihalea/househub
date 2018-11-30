using Microsoft.AspNetCore.Mvc.Rendering;
using System;

public static class _Layout
{
    public static string Index => "/Index";
    public static string ReviewProperties => "/Property/Review";
    public static string Create => "/Property/Create";
    public static string List => "/Property/Index";
    public static string User => "/User";
    public static string Certbot => "/Certbot/Index";

    public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);
    public static string ReviewNavClass(ViewContext viewContext) => PageNavClass(viewContext, ReviewProperties);
    public static string CreateNavClass(ViewContext viewContext) => PageNavClass(viewContext, Create);
    public static string ListNavClass(ViewContext viewContext) => PageNavClass(viewContext, List);
    public static string UsersNavClass(ViewContext viewContext) => FolderNavClass(viewContext, User);
    public static string CertbotNavClass(ViewContext viewContext) => PageNavClass(viewContext, Certbot);

    public static string PageNavClass(ViewContext viewContext, string page)
    {
        var activePage = viewContext.ViewData["ActivePage"] as string
                         ?? viewContext.ActionDescriptor.DisplayName;
        return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
    }

    public static string FolderNavClass(ViewContext viewContext, string folder)
    {
        var activePage = viewContext.ViewData["ActivePage"] as string
                         ?? viewContext.ActionDescriptor.DisplayName;
        var path = activePage.Substring(0, activePage.LastIndexOf("/"));

        return string.Equals(path, folder, StringComparison.OrdinalIgnoreCase) ? "active" : null;
    }
}