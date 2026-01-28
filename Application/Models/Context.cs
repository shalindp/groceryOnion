namespace Application.Models;

public record Context(WoolworthContext Woolworths);

public record WoolworthContext(WoolworthsRegionSession[] AreaSessions);
public record WoolworthsRegionSession(int RegeonId, WoolworthSession Session);
public record WoolworthSession(string SessionId, string Aga);
