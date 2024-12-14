using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Column("user_id")]
    public int UserId { get; set; } // Veritabanındaki "user_id" sütununa eşlenir

    public string Username { get; set; }
    public string Email { get; set; }

    [Column("password")]
    public string PasswordHash { get; set; }
    public string Role { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}
