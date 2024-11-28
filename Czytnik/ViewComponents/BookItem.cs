using Czytnik_Model.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

public class BookItem : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(BooksSearchViewModel element)
    {
        var book = element;
        return View("Default", book);
    }
}
