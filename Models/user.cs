using System.ComponentModel.DataAnnotations.Schema;

public enum UserRole
{
    Admin,
    User
}
public class User
{
    [Column("user_id")]
    public int UserId { get; set; }

    public string Username { get; set; }
    public string Email { get; set; }

    [Column("password")]
    public string PasswordHash { get; set; }

    [Column("role")]
    public UserRole Role { get; set; } // Enum olarak tanımlandı

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

}
