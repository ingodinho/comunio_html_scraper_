public static class UserMapper
{
  private static readonly Dictionary<string, User> userMap = new Dictionary<string, User> {
    {"12322012", User.Schgaup},
    {"12322001", User.Tammo},
    {"12322018", User.Boe},
    {"12321971", User.Ingo},
    {"12322212", User.Norbert},
    {"12322054", User.Jannes},
    {"12322144", User.Polly},
  };

  public static User ToUser(string userId)
  {
    if (!userMap.TryGetValue(userId, out User foundUser))
    {
      throw new Exception("No User found in mapping");
    }

    return foundUser;
  }
}