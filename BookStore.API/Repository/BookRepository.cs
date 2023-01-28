using AutoMapper;
using Azure;
using BookStore.API.Data;
using BookStore.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JsonPatchDocument = Microsoft.AspNetCore.JsonPatch.JsonPatchDocument;


namespace BookStore.API.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreContext _context;

        public IMapper _mapper { get; }

        public BookRepository(BookStoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<BookModel>> GetAllBooksAsync()
        {
            //var records = await _context.Books.Select(x => new BookModel() // converts Book to BookModel
            //{
            //    Id = x.Id,
            //    Title = x.Title,
            //    Description = x.Description
            //}).ToListAsync();

            //return records;

            var records = await _context.Books.ToListAsync();
            return _mapper.Map<List<BookModel>>(records);
        }

        public async Task<BookModel> GetBookByIdAsync(int bookId)
        {
            //var record = await _context.Books.Where(x => x.Id == bookId).Select(x => new BookModel() 
            //{
            //    Id = x.Id,
            //    Title = x.Title,
            //    Description = x.Description
            //}).FirstOrDefaultAsync();
            //return record;

            var book = await _context.Books.FindAsync(bookId);
            return _mapper.Map<BookModel>(book);

        }

        public async Task<int> AddBookAsync(BookModel book)
        {
            var newBook = new Book()
            {
                Title = book.Title,
                Description = book.Description
            };
            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();
            return newBook.Id;
        }
        
        public async Task UpdateBookAsync(int bookId, BookModel bookModel)
        {
            //var book = await _context.Books.FindAsync(bookId);
            //if (book != null)
            //{
            //    book.Title = bookModel.Title;
            //    book.Description = bookModel.Description;
            //    await _context.SaveChangesAsync();
            //}
            // we are hitting the database twice line 54 and 59 just to update a record - it can cause performance issues 
            var book = new Book()
            {
                Id = bookId,
                Title = bookModel.Title,
                Description = bookModel.Description
            };
            _context.Books.Update(book);
            await _context.SaveChangesAsync(); // hits the db only once 
        }

        public async Task UpdateBookPatchAsync(int bookId, JsonPatchDocument bookModel)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book != null)
            {
                bookModel.ApplyTo(book);
                await _context.SaveChangesAsync();

            }
        }

        public async Task DeleteBookAsync(int bookId)
        {
            var book = new Book() { Id = bookId };
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

    }
}
