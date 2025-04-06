namespace web_api.Exceptions;

public class NotAuthorizedException : Exception
{
    public NotAuthorizedException(string objName, string objId)
        : base($"You are not authorized to manipulate {objName} with id: {objId}")
    {
    }

    public NotAuthorizedException(string objName, long objId)
        : base($"You are not authorized to manipulate {objName} with id: {objId}")
    {
    }
}
