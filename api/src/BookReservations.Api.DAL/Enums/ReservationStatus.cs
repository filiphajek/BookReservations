namespace BookReservations.Api.DAL.Enums;

public enum ReservationStatus
{
    Created = 0, // user creates reservation
    Cancelled = 1, // user can cancel reservation before he picks up book in library, librarian can cancel it too .. book is not available etc.
    CanRetrieve = 2, // librarian confirms reservation
    Retrieved = 3, // user has book
    WantToExtend = 4, // user wants to extend reservation
    Extended = 5, // librarian confirms the request
    Returned = 6, // user returned book
    NotReturned = 7 // user did not returned book, librarian will contact him, user can pay fine 
}