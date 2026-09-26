namespace AddressManagement.Application.Exceptions;

public sealed class DuplicateAddressException() : Exception("An address with these details already exists.");