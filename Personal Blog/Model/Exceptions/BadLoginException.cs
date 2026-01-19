namespace Personal_Blog.Model.Exceptions;

public class BadLoginException(string msg) : Exception(msg);