using Czytnik.Services;
using Czytnik_Model.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class BooksCarousel : ViewComponent
{
    private readonly IBookService _bookService;

    public BooksCarousel(IBookService bookService)
    {
        _bookService = bookService;
    }
    public async Task<IViewComponentResult> InvokeAsync(string type, int series=-1,int category=-1, int book=-1)
    {
        IEnumerable<BooksCarouselViewModel> books;
        if (type == "Product") books = await _bookService.GetSimilarBooks(series, category, book);
        else books = await _bookService.GetTopMonthBooks(5, DateTime.Now);
        return View(type, books);
    }
}
