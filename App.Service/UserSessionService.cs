using App.Entity;
using App.Repository;

namespace App.Service;

public class UserSessionService : GenericService<UserSession>
{
    private readonly AccessActivityService activityService;

    public UserSessionService(AccessActivityService activityService, UserSessionRepository repository) :
        base(repository)
    {
        this.activityService = activityService;
    }

    public UserSession CreateNewUserSession(User user)
    {
        var userSession = new UserSession();
        userSession.StartTime = DateTime.UtcNow;
        userSession.IsActive = true;
        userSession.UserId = user.UserId;
        userSession.ExpirationTimeFrame = 45;
        userSession.SessionGuid = Guid.NewGuid();
        userSession.EndTime = DateTime.UtcNow.AddMinutes(45);
        Insert(userSession).Wait();
        return userSession;
    }

    public UserSession? GetByUserSessionGuid(Guid sid)
    {
        return GetAll().FirstOrDefault(p => p.SessionGuid == sid);
    }

    public void LogAccessActivity(User user, UserSession usersession, string ActivityType)
    {
        var obj = new AccessActivity()
        {
            UserId = user.UserId,
            UserSessionId = usersession.UserSessionId,
            ActivityType = ActivityType,
            CreatedOn = DateTime.UtcNow
        };
        activityService.Insert(obj).Wait();
    }

    public List<UserSession> GetByUser(int userId)
    {
        return GetAll().Where(p => p.UserId == userId && p.IsActive == true).ToList();
    }
}