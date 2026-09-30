using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication7
{
    public static class Database
    {
        private static readonly string DbName = "HeritageFameArtGallery";
        private static string MasterConnection
        {
            get { return @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True"; }
        }

        public static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["ArtGalleryDb"].ConnectionString; }
        }

        public static void Initialize()
        {
            // Create the LocalDB database if it does not exist.
            using (var master = new SqlConnection(MasterConnection))
            {
                master.Open();
                using (var cmd = new SqlCommand("IF DB_ID(@db) IS NULL CREATE DATABASE [HeritageFameArtGallery]", master))
                {
                    cmd.Parameters.AddWithValue("@db", DbName);
                    cmd.ExecuteNonQuery();
                }
            }

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string schema = @"
IF OBJECT_ID('dbo.AdminUsers','U') IS NULL
CREATE TABLE dbo.AdminUsers(
    AdminId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(100) NOT NULL);

IF OBJECT_ID('dbo.Users','U') IS NULL
CREATE TABLE dbo.Users(
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(30) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    LastLogin DATETIME NULL);

IF OBJECT_ID('dbo.Artworks','U') IS NULL
CREATE TABLE dbo.Artworks(
    ArtId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(150) NOT NULL,
    Artist NVARCHAR(150) NOT NULL,
    Category NVARCHAR(80) NOT NULL,
    Medium NVARCHAR(100) NOT NULL,
    Size NVARCHAR(80) NULL,
    Price DECIMAL(12,2) NOT NULL,
    ImageUrl NVARCHAR(1000) NOT NULL,
    Description NVARCHAR(1000) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE());

IF OBJECT_ID('dbo.Favorites','U') IS NULL
CREATE TABLE dbo.Favorites(
    FavoriteId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    ArtId INT NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Favorites_Users FOREIGN KEY(UserId) REFERENCES dbo.Users(UserId) ON DELETE CASCADE,
    CONSTRAINT FK_Favorites_Artworks FOREIGN KEY(ArtId) REFERENCES dbo.Artworks(ArtId) ON DELETE CASCADE,
    CONSTRAINT UQ_Favorites UNIQUE(UserId,ArtId));

IF OBJECT_ID('dbo.Cart','U') IS NULL
CREATE TABLE dbo.Cart(
    CartId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    ArtId INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,
    AddedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Cart_Users FOREIGN KEY(UserId) REFERENCES dbo.Users(UserId) ON DELETE CASCADE,
    CONSTRAINT FK_Cart_Artworks FOREIGN KEY(ArtId) REFERENCES dbo.Artworks(ArtId) ON DELETE CASCADE,
    CONSTRAINT UQ_Cart UNIQUE(UserId,ArtId));

IF OBJECT_ID('dbo.Orders','U') IS NULL
CREATE TABLE dbo.Orders(
    OrderId INT IDENTITY(1001,1) PRIMARY KEY,
    UserId INT NOT NULL,
    OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
    TotalAmount DECIMAL(12,2) NOT NULL,
    Status NVARCHAR(40) NOT NULL DEFAULT 'Order Placed',
    PaymentMethod NVARCHAR(40) NOT NULL,
    PaymentStatus NVARCHAR(40) NOT NULL DEFAULT 'Pending',
    TransactionId NVARCHAR(100) NULL,
    ShippingAddress NVARCHAR(500) NOT NULL,
    CONSTRAINT FK_Orders_Users FOREIGN KEY(UserId) REFERENCES dbo.Users(UserId));

IF OBJECT_ID('dbo.OrderDetails','U') IS NULL
CREATE TABLE dbo.OrderDetails(
    OrderDetailId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    ArtId INT NOT NULL,
    Title NVARCHAR(150) NOT NULL,
    Price DECIMAL(12,2) NOT NULL,
    Quantity INT NOT NULL,
    CONSTRAINT FK_OrderDetails_Orders FOREIGN KEY(OrderId) REFERENCES dbo.Orders(OrderId) ON DELETE CASCADE,
    CONSTRAINT FK_OrderDetails_Artworks FOREIGN KEY(ArtId) REFERENCES dbo.Artworks(ArtId));

IF OBJECT_ID('dbo.OrderTracking','U') IS NULL
CREATE TABLE dbo.OrderTracking(
    TrackingId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    Status NVARCHAR(40) NOT NULL,
    Note NVARCHAR(300) NULL,
    UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_OrderTracking_Orders FOREIGN KEY(OrderId) REFERENCES dbo.Orders(OrderId) ON DELETE CASCADE);

IF OBJECT_ID('dbo.ActivityLog','U') IS NULL
CREATE TABLE dbo.ActivityLog(
    LogId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NULL,
    AdminId INT NULL,
    Action NVARCHAR(100) NOT NULL,
    Details NVARCHAR(500) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE());

IF NOT EXISTS (SELECT 1 FROM dbo.AdminUsers WHERE Username=N'admin')
    INSERT INTO dbo.AdminUsers(Username,PasswordHash) VALUES(N'admin',N'admin123');

-- Remove only the old demo records created by earlier versions of this project.
-- Never remove an artwork that is referenced by an existing order.
DELETE FROM dbo.Artworks
WHERE Artist IN (N'Heritage Fame Collection',N'Legacy Order Archive')
  AND NOT EXISTS (SELECT 1 FROM dbo.OrderDetails od WHERE od.ArtId=dbo.Artworks.ArtId);
";
                using (var cmd = new SqlCommand(schema, con)) cmd.ExecuteNonQuery();
            }

            // Preserve the previous Explore Gallery records. They are moved out of the normal gallery so existing orders remain valid.
            const string moveOldSoulmate = @"UPDATE dbo.Artworks SET Category=N'Explore Gallery' WHERE Category=N'Soulmate' AND ImageUrl IN (N'https://i.pinimg.com/736x/b8/2b/78/b82b7878a869f7db55a3730a1a0638a9.jpg',N'https://i.pinimg.com/736x/eb/e7/59/ebe759bfa5403bf871c57f8e350f63a8.jpg',N'https://i.pinimg.com/736x/f4/b0/a0/f4b0a0ca7ef23f612c29d94baefd79ed.jpg',N'https://i.pinimg.com/736x/14/d0/55/14d0551ed54e2235693478ecd6e16e7d.jpg',N'https://i.pinimg.com/736x/b9/54/a7/b954a7a95e3946fbd6bac2f41302e9c8.jpg',N'https://i.pinimg.com/736x/7b/b3/9e/7bb39eb0ba1a96a25be3ea83306959d6.jpg',N'https://i.pinimg.com/1200x/5c/29/d9/5c29d9f91a93f9f0b09fcdfb0cb17dd6.jpg',N'https://i.pinimg.com/1200x/82/77/36/8277369f1516137870a72cd6810c6f69.jpg');";
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand(moveOldSoulmate, con)) cmd.ExecuteNonQuery();
            }

            SeedCatalog();
        }

        private static void SeedCatalog()
        {
            var paintings = new[]
            {
                new object[]{"Whispering Meadow","Heritage Fame Studio","Nature","Landscape Painting","24 × 36 in",2400m,"https://i.pinimg.com/1200x/22/04/e5/2204e586ef5db78623cbfed4ce0225d8.jpg","A peaceful natural scene that captures the quiet beauty of an open meadow and the calm of the countryside."},
                new object[]{"Golden Mountain Morning","Heritage Fame Studio","Nature","Landscape Painting","24 × 36 in",2800m,"https://i.pinimg.com/736x/72/6c/97/726c9736d3cc8a873372004530d2ae20.jpg","A warm mountain landscape filled with soft light, layered hills and the peaceful atmosphere of a new morning."},
                new object[]{"Blue Horizon","Heritage Fame Studio","Nature","Landscape Painting","24 × 36 in",2600m,"https://i.pinimg.com/736x/d2/14/08/d21408a6c60b603d868ace5a745ee3c2.jpg","A serene horizon where sky and landscape meet, created to bring a feeling of openness and tranquillity."},
                new object[]{"Forest After Rain","Heritage Fame Studio","Nature","Landscape Painting","24 × 36 in",2500m,"https://i.pinimg.com/1200x/2c/6c/d3/2c6cd3cd16c4cde06bc4c0b7aa562643.jpg","A lush forest scene inspired by the freshness and stillness that follows a gentle rainfall."},
                new object[]{"Quiet Valley","Heritage Fame Studio","Nature","Landscape Painting","24 × 36 in",2700m,"https://i.pinimg.com/736x/a1/2a/65/a12a655f2da5514816b37d93070168d5.jpg","A tranquil valley landscape with a soft, scenic composition that invites the viewer to pause and explore."},
                new object[]{"Sunlit Wilderness","Heritage Fame Studio","Nature","Landscape Painting","24 × 36 in",2900m,"https://i.pinimg.com/1200x/b2/38/67/b23867171edb6ae83b284bc32609d932.jpg","A vibrant natural landscape celebrating sunlight, open space and the beauty of untouched surroundings."},
                new object[]{"Nature's Calm","Heritage Fame Studio","Nature","Landscape Painting","24 × 36 in",2300m,"https://i.pinimg.com/1200x/e3/e5/42/e3e54278d53aa2106aeb062d92d537d9.jpg","A calm composition inspired by nature's quiet moments, designed to create a restful mood."},

                new object[]{"Fragments of Thought","Heritage Fame Studio","Abstract","Abstract Art","24 × 36 in",2200m,"https://i.pinimg.com/736x/8f/33/54/8f3354e21ba4d5d254f6e208845dc4a2.jpg","An expressive abstract composition that turns colour, shape and movement into a visual exploration of thought."},
                new object[]{"Rhythm in Colour","Heritage Fame Studio","Abstract","Abstract Art","24 × 36 in",2500m,"https://i.pinimg.com/1200x/47/22/e3/4722e31279bbb94e8a7adf19788660b1.jpg","Bold visual rhythms and layered colours create a dynamic abstract work full of energy and movement."},
                new object[]{"Dream Geometry","Heritage Fame Studio","Abstract","Abstract Art","24 × 36 in",2400m,"https://i.pinimg.com/736x/95/f3/4d/95f34da3d618cad0ff61bf05203dce01.jpg","A playful study of shapes and colour that blends geometric structure with an imaginative dreamlike mood."},
                new object[]{"Inner Motion","Heritage Fame Studio","Abstract","Abstract Art","24 × 36 in",2600m,"https://i.pinimg.com/1200x/82/0a/e8/820ae8511a962e2dd0e753da9b2ee6c3.jpg","An abstract interpretation of movement and emotion expressed through layered forms and contrasting visual elements."},
                new object[]{"Colour Conversations","Heritage Fame Studio","Abstract","Abstract Art","24 × 36 in",2300m,"https://i.pinimg.com/736x/35/5d/0b/355d0b0412aa1a420571134912f04fd8.jpg","Colours interact across the canvas to create a lively visual conversation that can be interpreted in many ways."},
                new object[]{"Beyond the Frame","Heritage Fame Studio","Abstract","Abstract Art","24 × 36 in",2700m,"https://i.pinimg.com/1200x/d3/28/62/d328628ca26d3995e872f7aad3da4bbb.jpg","A contemporary abstract work suggesting imagination beyond ordinary boundaries and familiar forms."},
                new object[]{"Abstract Reverie","Heritage Fame Studio","Abstract","Abstract Art","24 × 36 in",2450m,"https://i.pinimg.com/1200x/cc/de/82/ccde8205f30f08a56ded3a99d2d69545.jpg","A dreamlike abstract composition balancing colour, texture and form to create a reflective visual experience."},

                new object[]{"Majestic Wild","Heritage Fame Studio","Wildlife","Wildlife Painting","24 × 36 in",3000m,"https://i.pinimg.com/736x/97/b0/d5/97b0d5c459590392cc348f7ace5371c3.jpg","A powerful wildlife composition celebrating the beauty, strength and presence of animals in their natural world."},
                new object[]{"Spirit of the Forest","Heritage Fame Studio","Wildlife","Wildlife Painting","24 × 36 in",3200m,"https://i.pinimg.com/1200x/b8/73/de/b873de6f5bc2102404acf8360e963f22.jpg","A dramatic forest-inspired artwork that highlights the mystery and character of wildlife."},
                new object[]{"Eyes of the Wild","Heritage Fame Studio","Wildlife","Wildlife Painting","24 × 36 in",3100m,"https://i.pinimg.com/1200x/86/70/e9/8670e953ea3bef2f81395c184a288680.jpg","A close and expressive wildlife study focused on the character and alertness of the natural world."},
                new object[]{"Wild Freedom","Heritage Fame Studio","Wildlife","Wildlife Painting","24 × 36 in",2900m,"https://i.pinimg.com/736x/93/a5/fa/93a5fa83cfe9e5702f64ea15b5c7ea9c.jpg","A celebration of movement and freedom inspired by animals living beyond the boundaries of the city."},
                new object[]{"Untamed Beauty","Heritage Fame Studio","Wildlife","Wildlife Painting","24 × 36 in",3300m,"https://i.pinimg.com/1200x/40/d9/cd/40d9cdc11fdf24be98730367795f222d.jpg","A richly composed wildlife scene portraying the raw beauty and diversity of nature."},
                new object[]{"Silent Guardian","Heritage Fame Studio","Wildlife","Wildlife Painting","24 × 36 in",3050m,"https://i.pinimg.com/1200x/ef/fb/59/effb59b6948c7210c75f5fc90fb7c9cf.jpg","A dignified wildlife portrait that conveys strength, patience and quiet presence."},
                new object[]{"Wild Kingdom","Heritage Fame Studio","Wildlife","Wildlife Painting","24 × 36 in",3150m,"https://i.pinimg.com/736x/25/28/98/252898940700ae07f4abff0f5e8897bc.jpg","A vibrant work inspired by the richness and variety of the animal kingdom."},

                new object[]{"Divine Serenity","Heritage Fame Studio","Religious","Religious Art","24 × 36 in",2800m,"https://i.pinimg.com/736x/27/69/b5/2769b5fc6391bd3e3c5e12f3a7b39768.jpg","A serene devotional composition created to evoke peace, reflection and spiritual warmth."},
                new object[]{"Sacred Blessing","Heritage Fame Studio","Religious","Religious Art","24 × 36 in",2900m,"https://i.pinimg.com/736x/56/18/22/5618224724984aa17b59d14316d6124f.jpg","A devotional artwork inspired by blessing, faith and the comfort of sacred traditions."},
                new object[]{"Light of Faith","Heritage Fame Studio","Religious","Religious Art","24 × 36 in",3000m,"https://i.pinimg.com/1200x/f1/5a/ab/f15aab440f222fd935e0a6ab7ed55ba6.jpg","A luminous spiritual composition representing faith, hope and inner peace."},
                new object[]{"Sacred Presence","Heritage Fame Studio","Religious","Religious Art","24 × 36 in",3100m,"https://i.pinimg.com/1200x/6f/73/73/6f7373d77999ab6b6948ccbd5a02d8a4.jpg","A contemplative work inspired by the quiet presence and symbolism of religious art."},
                new object[]{"Prayerful Heart","Heritage Fame Studio","Religious","Religious Art","24 × 36 in",2700m,"https://i.pinimg.com/736x/1c/6a/a2/1c6aa2fbc6dec75f82eaa1a4bc98e799.jpg","A heartfelt devotional scene expressing prayer, devotion and a sense of connection."},
                new object[]{"Path of Devotion","Heritage Fame Studio","Religious","Religious Art","24 × 36 in",2850m,"https://i.pinimg.com/736x/83/56/96/8356962ac1b6bfcd252cd1b2519db88f.jpg","An artwork inspired by the journey of devotion and the values carried through generations."},
                new object[]{"Temple of Peace","Heritage Fame Studio","Religious","Religious Art","24 × 36 in",2950m,"https://i.pinimg.com/736x/fd/7d/52/fd7d52c618428ff7b256067cc0b47dee.jpg","A peaceful religious composition celebrating sacred spaces, tradition and quiet reflection."},
                new object[]{"Eternal Blessings","Heritage Fame Studio","Religious","Religious Art","24 × 36 in",3050m,"https://i.pinimg.com/1200x/82/23/b0/8223b025469e885508fba6b43c00950f.jpg","A warm devotional artwork representing blessings, continuity and spiritual connection."},

                new object[]{"Moonlit Embrace","Heritage Fame Studio","Soulmate","Romantic Painting","24 × 36 in",3200m,"https://i.pinimg.com/736x/72/d4/07/72d4075052905e815be26eeda6ede561.jpg","A tender romantic scene filled with soft light and a quiet sense of closeness, capturing the warmth of two hearts drawn together."},
                new object[]{"Starlit Promise","Heritage Fame Studio","Soulmate","Romantic Painting","24 × 36 in",3400m,"https://i.pinimg.com/736x/2e/2f/9e/2e2f9ef72e81666380369c066693f181.jpg","A dreamy evening romance beneath a glowing sky, expressing affection, intimacy and the feeling of sharing a special moment together."},
                new object[]{"Golden Affection","Heritage Fame Studio","Soulmate","Romantic Painting","24 × 36 in",3500m,"https://i.pinimg.com/1200x/c1/d5/de/c1d5de627c0f5d3bc896e9ff00021726.jpg","A warm portrait inspired by admiration and affection, with rich golden tones creating an intimate and elegant romantic mood."},
                new object[]{"A Promise of Love","Heritage Fame Studio","Soulmate","Romantic Painting","24 × 36 in",3000m,"https://i.pinimg.com/736x/e7/98/0a/e7980a0d22669354ecaedf468ff7f23b.jpg","A gentle romantic composition that conveys trust, tenderness and the quiet beauty of a meaningful connection."},
                new object[]{"Romance in Bloom","Heritage Fame Studio","Soulmate","Romantic Painting","24 × 36 in",3300m,"https://i.pinimg.com/736x/c7/c5/11/c7c511dc020f7f6e1cd5528ec76edb4f.jpg","A love-inspired artwork where floral beauty and romantic feeling come together in a soft, expressive composition."},
                new object[]{"Tender Hearts","Heritage Fame Studio","Soulmate","Romantic Painting","24 × 36 in",3600m,"https://i.pinimg.com/736x/ef/2d/20/ef2d20f25a7a4bdda448c1702910db33.jpg","A soft and emotional romantic artwork celebrating closeness, affection and the gentle bond between two people."},
                new object[]{"Love in Elegance","Heritage Fame Studio","Soulmate","Romantic Painting","24 × 36 in",3700m,"https://i.pinimg.com/736x/82/87/c8/8287c8acc90a08ccfa944ffbbae0af79.jpg","An elegant romantic composition expressing companionship and affection through graceful figures, colour and atmosphere."},
                new object[]{"Under the Moonlight","Heritage Fame Studio","Soulmate","Romantic Painting","24 × 36 in",3100m,"https://i.pinimg.com/736x/12/58/36/125836a5be5f0859d546461c9dda2bc1.jpg","A calm romantic scene inspired by moonlight, connection and the peaceful feeling of sharing a beautiful moment with someone special."},
            };

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                foreach (object[] p in paintings)
                {
                    const string sql = @"
IF EXISTS (SELECT 1 FROM dbo.Artworks WHERE ImageUrl=@ImageUrl)
    UPDATE dbo.Artworks SET Title=@Title,Artist=@Artist,Category=@Category,Medium=@Medium,Size=@Size,Price=@Price,Description=@Description WHERE ImageUrl=@ImageUrl;
ELSE
    INSERT INTO dbo.Artworks(Title,Artist,Category,Medium,Size,Price,ImageUrl,Description) VALUES(@Title,@Artist,@Category,@Medium,@Size,@Price,@ImageUrl,@Description);";
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Title", p[0]);
                        cmd.Parameters.AddWithValue("@Artist", p[1]);
                        cmd.Parameters.AddWithValue("@Category", p[2]);
                        cmd.Parameters.AddWithValue("@Medium", p[3]);
                        cmd.Parameters.AddWithValue("@Size", p[4]);
                        cmd.Parameters.AddWithValue("@Price", p[5]);
                        cmd.Parameters.AddWithValue("@ImageUrl", p[6]);
                        cmd.Parameters.AddWithValue("@Description", p[7]);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private static string Hash(string value)
        {
            using (var sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(value ?? ""));
                var sb = new StringBuilder();
                foreach (byte b in bytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        public static DataTable GetArtworks(string search = "", string category = "")
        {
            using (var con = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(@"SELECT ArtId,Title,Artist,Category,Medium,Size,Price,ImageUrl,Description
                FROM Artworks WHERE Artist NOT IN (N'Heritage Fame Collection',N'Legacy Order Archive') AND Category<>N'Explore Gallery'
                AND (@Search='' OR Title LIKE '%' + @Search + '%' OR Artist LIKE '%' + @Search + '%')
                AND (@Category='' OR Category=@Category) ORDER BY ArtId DESC", con))
            {
                cmd.Parameters.AddWithValue("@Search", search ?? "");
                cmd.Parameters.AddWithValue("@Category", category ?? "");
                var dt = new DataTable();
                new SqlDataAdapter(cmd).Fill(dt);
                return dt;
            }
        }

        public static DataRow GetArtwork(int id)
        {
            using (var con = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand("SELECT TOP 1 * FROM Artworks WHERE ArtId=@Id", con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                var dt = new DataTable();
                new SqlDataAdapter(cmd).Fill(dt);
                return dt.Rows.Count == 0 ? null : dt.Rows[0];
            }
        }

        public static void AddArtwork(string title,string artist,string category,string medium,string size,decimal price,string imageUrl,string description)
        {
            ExecuteArtworkCommand(@"INSERT INTO Artworks(Title,Artist,Category,Medium,Size,Price,ImageUrl,Description)
                VALUES(@Title,@Artist,@Category,@Medium,@Size,@Price,@ImageUrl,@Description)",null,title,artist,category,medium,size,price,imageUrl,description);
        }

        public static void UpdateArtwork(int id,string title,string artist,string category,string medium,string size,decimal price,string imageUrl,string description)
        {
            ExecuteArtworkCommand(@"UPDATE Artworks SET Title=@Title,Artist=@Artist,Category=@Category,Medium=@Medium,Size=@Size,Price=@Price,ImageUrl=@ImageUrl,Description=@Description WHERE ArtId=@Id",
                id,title,artist,category,medium,size,price,imageUrl,description);
        }

        public static bool DeleteArtwork(int id)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                // An artwork that has already been purchased must remain in the database
                // because OrderDetails keeps its ArtId for order history.
                using (var check = new SqlCommand(
                    "SELECT COUNT(1) FROM dbo.OrderDetails WHERE ArtId=@Id", con))
                {
                    check.Parameters.AddWithValue("@Id", id);
                    int orderCount = Convert.ToInt32(check.ExecuteScalar());
                    if (orderCount > 0) return false;
                }

                // Favorites and Cart use ON DELETE CASCADE, so they are removed automatically.
                using (var cmd = new SqlCommand("DELETE FROM dbo.Artworks WHERE ArtId=@Id", con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        private static void ExecuteArtworkCommand(string sql,int? id,string title,string artist,string category,string medium,string size,decimal price,string imageUrl,string description)
        {
            using (var con=new SqlConnection(ConnectionString))
            using (var cmd=new SqlCommand(sql,con))
            {
                if(id.HasValue) cmd.Parameters.AddWithValue("@Id",id.Value);
                cmd.Parameters.AddWithValue("@Title",title);
                cmd.Parameters.AddWithValue("@Artist",artist);
                cmd.Parameters.AddWithValue("@Category",category);
                cmd.Parameters.AddWithValue("@Medium",medium);
                cmd.Parameters.AddWithValue("@Size",(object)size ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Price",price);
                cmd.Parameters.AddWithValue("@ImageUrl",imageUrl);
                cmd.Parameters.AddWithValue("@Description",(object)description ?? DBNull.Value);
                con.Open(); cmd.ExecuteNonQuery();
            }
        }

        public static bool ValidateAdmin(string username,string password)
        {
            using(var con=new SqlConnection(ConnectionString))
            using(var cmd=new SqlCommand("SELECT COUNT(*) FROM AdminUsers WHERE Username=@Username AND PasswordHash=@Password",con))
            {
                cmd.Parameters.AddWithValue("@Username",username);
                cmd.Parameters.AddWithValue("@Password",password);
                con.Open(); return (int)cmd.ExecuteScalar()==1;
            }
        }

        public static int CreateUser(string fullName,string email,string password,string phone)
        {
            using(var con=new SqlConnection(ConnectionString))
            using(var cmd=new SqlCommand(@"INSERT INTO Users(FullName,Email,PasswordHash,Phone) VALUES(@Name,@Email,@Password,@Phone); SELECT CAST(SCOPE_IDENTITY() AS INT);",con))
            {
                cmd.Parameters.AddWithValue("@Name",fullName);
                cmd.Parameters.AddWithValue("@Email",email);
                cmd.Parameters.AddWithValue("@Password",Hash(password));
                cmd.Parameters.AddWithValue("@Phone",(object)phone ?? DBNull.Value);
                try { con.Open(); int id=(int)cmd.ExecuteScalar(); Log(id,null,"Signup","New user account created"); return id; }
                catch(SqlException ex) when(ex.Number==2601 || ex.Number==2627) { return -1; }
            }
        }

        public static int ValidateUser(string email, string password)
        {
            email = (email ?? string.Empty).Trim();
            if (email.Length == 0) return 0;

            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                const string sql = @"SELECT TOP 1 UserId,PasswordHash FROM dbo.Users WHERE LOWER(LTRIM(RTRIM(Email)))=LOWER(@Email)";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    using (var rr = cmd.ExecuteReader())
                    {
                        if (!rr.Read()) return 0;
                        int id = Convert.ToInt32(rr["UserId"]);
                        string stored = rr["PasswordHash"].ToString();
                        string hashed = Hash(password ?? string.Empty);
                        if (!string.Equals(stored, hashed, StringComparison.OrdinalIgnoreCase) && stored != (password ?? string.Empty)) return 0;

                        rr.Close();
                        using (var update = new SqlCommand("UPDATE dbo.Users SET LastLogin=GETDATE(),PasswordHash=@Hash WHERE UserId=@Id", con))
                        {
                            update.Parameters.AddWithValue("@Hash", hashed);
                            update.Parameters.AddWithValue("@Id", id);
                            update.ExecuteNonQuery();
                        }
                        Log(id, null, "Login", "User login successful");
                        return id;
                    }
                }
            }
        }

        public static DataTable GetUser(int userId)
        {
            using(var con=new SqlConnection(ConnectionString))
            using(var cmd=new SqlCommand("SELECT UserId,FullName,Email,Phone FROM Users WHERE UserId=@Id",con))
            { cmd.Parameters.AddWithValue("@Id",userId); var dt=new DataTable(); new SqlDataAdapter(cmd).Fill(dt); return dt; }
        }

        public static bool IsFavorite(int userId,int artId)
        {
            using(var con=new SqlConnection(ConnectionString))
            using(var cmd=new SqlCommand("SELECT COUNT(*) FROM Favorites WHERE UserId=@UserId AND ArtId=@ArtId",con))
            { cmd.Parameters.AddWithValue("@UserId",userId); cmd.Parameters.AddWithValue("@ArtId",artId); con.Open(); return (int)cmd.ExecuteScalar()>0; }
        }

        public static void ToggleFavorite(int userId,int artId)
        {
            using(var con=new SqlConnection(ConnectionString))
            {
                con.Open();
                using(var check=new SqlCommand("SELECT COUNT(*) FROM Favorites WHERE UserId=@UserId AND ArtId=@ArtId",con))
                {
                    check.Parameters.AddWithValue("@UserId",userId); check.Parameters.AddWithValue("@ArtId",artId);
                    bool exists=(int)check.ExecuteScalar()>0;
                    string sql=exists ? "DELETE FROM Favorites WHERE UserId=@UserId AND ArtId=@ArtId" : "INSERT INTO Favorites(UserId,ArtId) VALUES(@UserId,@ArtId)";
                    using(var cmd=new SqlCommand(sql,con))
                    { cmd.Parameters.AddWithValue("@UserId",userId); cmd.Parameters.AddWithValue("@ArtId",artId); cmd.ExecuteNonQuery(); }
                    Log(userId,null,exists?"Remove Favorite":"Add Favorite","ArtId="+artId);
                }
            }
        }

        public static DataTable GetFavorites(int userId)
        {
            using(var con=new SqlConnection(ConnectionString))
            using(var cmd=new SqlCommand(@"SELECT f.FavoriteId,a.ArtId,a.Title,a.Artist,a.Category,a.Price,a.ImageUrl
                FROM Favorites f INNER JOIN Artworks a ON f.ArtId=a.ArtId WHERE f.UserId=@UserId ORDER BY f.FavoriteId DESC",con))
            { cmd.Parameters.AddWithValue("@UserId",userId); var dt=new DataTable(); new SqlDataAdapter(cmd).Fill(dt); return dt; }
        }

        public static void AddToCart(int userId,int artId)
        {
            using(var con=new SqlConnection(ConnectionString))
            {
                con.Open();
                using(var cmd=new SqlCommand(@"IF EXISTS(SELECT 1 FROM Cart WHERE UserId=@UserId AND ArtId=@ArtId)
                    UPDATE Cart SET Quantity=Quantity+1 WHERE UserId=@UserId AND ArtId=@ArtId
                    ELSE INSERT INTO Cart(UserId,ArtId,Quantity) VALUES(@UserId,@ArtId,1)",con))
                { cmd.Parameters.AddWithValue("@UserId",userId); cmd.Parameters.AddWithValue("@ArtId",artId); cmd.ExecuteNonQuery(); }
                Log(userId,null,"Add to Cart","ArtId="+artId);
            }
        }

        public static DataTable GetCart(int userId)
        {
            using(var con=new SqlConnection(ConnectionString))
            using(var cmd=new SqlCommand(@"SELECT c.CartId,c.ArtId,a.Title,a.Artist,a.Price,c.Quantity,(a.Price*c.Quantity) LineTotal,a.ImageUrl
                FROM Cart c INNER JOIN Artworks a ON c.ArtId=a.ArtId WHERE c.UserId=@UserId ORDER BY c.CartId",con))
            { cmd.Parameters.AddWithValue("@UserId",userId); var dt=new DataTable(); new SqlDataAdapter(cmd).Fill(dt); return dt; }
        }

        public static void UpdateCartQuantity(int userId,int cartId,int quantity)
        {
            if(quantity<1) quantity=1;
            using(var con=new SqlConnection(ConnectionString))
            using(var cmd=new SqlCommand("UPDATE Cart SET Quantity=@Qty WHERE CartId=@CartId AND UserId=@UserId",con))
            { cmd.Parameters.AddWithValue("@Qty",quantity); cmd.Parameters.AddWithValue("@CartId",cartId); cmd.Parameters.AddWithValue("@UserId",userId); con.Open(); cmd.ExecuteNonQuery(); }
            Log(userId,null,"Update Cart","CartId="+cartId+" Quantity="+quantity);
        }

        public static void RemoveCart(int userId,int cartId)
        {
            using(var con=new SqlConnection(ConnectionString))
            using(var cmd=new SqlCommand("DELETE FROM Cart WHERE CartId=@CartId AND UserId=@UserId",con))
            { cmd.Parameters.AddWithValue("@CartId",cartId); cmd.Parameters.AddWithValue("@UserId",userId); con.Open(); cmd.ExecuteNonQuery(); }
            Log(userId,null,"Remove from Cart","CartId="+cartId);
        }

        public static decimal GetCartTotal(int userId)
        {
            using(var con=new SqlConnection(ConnectionString))
            using(var cmd=new SqlCommand("SELECT ISNULL(SUM(a.Price*c.Quantity),0) FROM Cart c INNER JOIN Artworks a ON c.ArtId=a.ArtId WHERE c.UserId=@UserId",con))
            { cmd.Parameters.AddWithValue("@UserId",userId); con.Open(); return Convert.ToDecimal(cmd.ExecuteScalar()); }
        }

        public static int CreateOrder(int userId,string address,string paymentMethod,string paymentStatus)
        {
            using(var con=new SqlConnection(ConnectionString))
            {
                con.Open();
                var tx=con.BeginTransaction();
                try
                {
                    DataTable cart=new DataTable();
                    using(var cmd=new SqlCommand(@"SELECT c.ArtId,a.Title,a.Price,c.Quantity,(a.Price*c.Quantity) LineTotal
                        FROM Cart c INNER JOIN Artworks a ON c.ArtId=a.ArtId WHERE c.UserId=@UserId",con,tx))
                    { cmd.Parameters.AddWithValue("@UserId",userId); new SqlDataAdapter(cmd).Fill(cart); }
                    if(cart.Rows.Count==0) { tx.Rollback(); return 0; }

                    decimal total=0;
                    foreach(DataRow r in cart.Rows) total+=Convert.ToDecimal(r["LineTotal"]);

                    int orderId;
                    using(var cmd=new SqlCommand(@"INSERT INTO Orders(UserId,TotalAmount,Status,PaymentMethod,PaymentStatus,TransactionId,ShippingAddress)
                        VALUES(@UserId,@Total,'Order Placed',@Method,@PaymentStatus,@TransactionId,@Address); SELECT CAST(SCOPE_IDENTITY() AS INT);",con,tx))
                    {
                        cmd.Parameters.AddWithValue("@UserId",userId);
                        cmd.Parameters.AddWithValue("@Total",total);
                        cmd.Parameters.AddWithValue("@Method",paymentMethod);
                        cmd.Parameters.AddWithValue("@PaymentStatus",paymentStatus);
                        cmd.Parameters.AddWithValue("@TransactionId",paymentStatus=="Paid" ? "DEMO-"+DateTime.Now.ToString("yyyyMMddHHmmssfff") : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Address",address);
                        orderId=(int)cmd.ExecuteScalar();
                    }

                    foreach(DataRow r in cart.Rows)
                    {
                        using(var cmd=new SqlCommand(@"INSERT INTO OrderDetails(OrderId,ArtId,Title,Price,Quantity) VALUES(@OrderId,@ArtId,@Title,@Price,@Qty)",con,tx))
                        {
                            cmd.Parameters.AddWithValue("@OrderId",orderId);
                            cmd.Parameters.AddWithValue("@ArtId",r["ArtId"]);
                            cmd.Parameters.AddWithValue("@Title",r["Title"]);
                            cmd.Parameters.AddWithValue("@Price",r["Price"]);
                            cmd.Parameters.AddWithValue("@Qty",r["Quantity"]);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    using(var cmd=new SqlCommand("INSERT INTO OrderTracking(OrderId,Status,Note) VALUES(@OrderId,'Order Placed','Your order has been placed successfully.')",con,tx))
                    { cmd.Parameters.AddWithValue("@OrderId",orderId); cmd.ExecuteNonQuery(); }

                    using(var cmd=new SqlCommand("DELETE FROM Cart WHERE UserId=@UserId",con,tx))
                    { cmd.Parameters.AddWithValue("@UserId",userId); cmd.ExecuteNonQuery(); }

                    tx.Commit();
                    Log(userId,null,"Order Created","OrderId="+orderId+" Payment="+paymentMethod+" "+paymentStatus);
                    return orderId;
                }
                catch { tx.Rollback(); throw; }
            }
        }

        public static DataTable GetOrders(int userId)
        {
            using(var con=new SqlConnection(ConnectionString))
            using(var cmd=new SqlCommand("SELECT OrderId,OrderDate,TotalAmount,Status,PaymentMethod,PaymentStatus,ShippingAddress FROM Orders WHERE UserId=@UserId ORDER BY OrderDate DESC",con))
            { cmd.Parameters.AddWithValue("@UserId",userId); var dt=new DataTable(); new SqlDataAdapter(cmd).Fill(dt); return dt; }
        }

        public static DataTable GetOrder(int userId,int orderId)
        {
            using(var con=new SqlConnection(ConnectionString))
            using(var cmd=new SqlCommand("SELECT OrderId,OrderDate,TotalAmount,Status,PaymentMethod,PaymentStatus,ShippingAddress FROM Orders WHERE UserId=@UserId AND OrderId=@OrderId",con))
            { cmd.Parameters.AddWithValue("@UserId",userId); cmd.Parameters.AddWithValue("@OrderId",orderId); var dt=new DataTable(); new SqlDataAdapter(cmd).Fill(dt); return dt; }
        }

        public static DataTable GetOrderItems(int userId,int orderId)
        {
            using(var con=new SqlConnection(ConnectionString))
            using(var cmd=new SqlCommand(@"SELECT d.Title,d.Price,d.Quantity,(d.Price*d.Quantity) LineTotal FROM OrderDetails d
                INNER JOIN Orders o ON d.OrderId=o.OrderId WHERE o.UserId=@UserId AND d.OrderId=@OrderId",con))
            { cmd.Parameters.AddWithValue("@UserId",userId); cmd.Parameters.AddWithValue("@OrderId",orderId); var dt=new DataTable(); new SqlDataAdapter(cmd).Fill(dt); return dt; }
        }

        public static DataTable GetTracking(int userId,int orderId)
        {
            using(var con=new SqlConnection(ConnectionString))
            using(var cmd=new SqlCommand(@"SELECT t.Status,t.Note,t.UpdatedAt FROM OrderTracking t INNER JOIN Orders o ON t.OrderId=o.OrderId
                WHERE o.UserId=@UserId AND t.OrderId=@OrderId ORDER BY t.UpdatedAt",con))
            { cmd.Parameters.AddWithValue("@UserId",userId); cmd.Parameters.AddWithValue("@OrderId",orderId); var dt=new DataTable(); new SqlDataAdapter(cmd).Fill(dt); return dt; }
        }

        public static int GetAdminId(string username)
        {
            using(var con=new SqlConnection(ConnectionString))
            using(var cmd=new SqlCommand("SELECT AdminId FROM AdminUsers WHERE Username=@Username",con))
            { cmd.Parameters.AddWithValue("@Username",username); con.Open(); object x=cmd.ExecuteScalar(); return x==null?0:Convert.ToInt32(x); }
        }

        public static DataTable GetAllOrders()
        {
            using(var con=new SqlConnection(ConnectionString))
            using(var cmd=new SqlCommand(@"SELECT o.OrderId,o.OrderDate,u.FullName,u.Email,o.TotalAmount,o.Status,o.PaymentMethod,o.PaymentStatus,o.ShippingAddress
                FROM Orders o INNER JOIN Users u ON o.UserId=u.UserId ORDER BY o.OrderDate DESC",con))
            { var dt=new DataTable(); new SqlDataAdapter(cmd).Fill(dt); return dt; }
        }

        public static void UpdateOrderStatus(int adminId,int orderId,string status,string note)
        {
            using(var con=new SqlConnection(ConnectionString))
            {
                con.Open(); var tx=con.BeginTransaction();
                try
                {
                    int userId;
                    using(var cmd=new SqlCommand("SELECT UserId FROM Orders WHERE OrderId=@OrderId",con,tx))
                    { cmd.Parameters.AddWithValue("@OrderId",orderId); object x=cmd.ExecuteScalar(); if(x==null){tx.Rollback();return;} userId=Convert.ToInt32(x); }
                    using(var cmd=new SqlCommand("UPDATE Orders SET Status=@Status WHERE OrderId=@OrderId",con,tx))
                    { cmd.Parameters.AddWithValue("@Status",status); cmd.Parameters.AddWithValue("@OrderId",orderId); cmd.ExecuteNonQuery(); }
                    using(var cmd=new SqlCommand("INSERT INTO OrderTracking(OrderId,Status,Note) VALUES(@OrderId,@Status,@Note)",con,tx))
                    { cmd.Parameters.AddWithValue("@OrderId",orderId);cmd.Parameters.AddWithValue("@Status",status);cmd.Parameters.AddWithValue("@Note",note);cmd.ExecuteNonQuery(); }
                    tx.Commit(); Log(userId,adminId,"Update Order Status","OrderId="+orderId+" Status="+status);
                }
                catch { tx.Rollback(); throw; }
            }
        }

        public static void Log(int? userId,int? adminId,string action,string details)
        {
            using(var con=new SqlConnection(ConnectionString))
            using(var cmd=new SqlCommand("INSERT INTO ActivityLog(UserId,AdminId,Action,Details) VALUES(@UserId,@AdminId,@Action,@Details)",con))
            {
                cmd.Parameters.AddWithValue("@UserId",(object)userId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@AdminId",(object)adminId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Action",action);
                cmd.Parameters.AddWithValue("@Details",(object)details ?? DBNull.Value);
                con.Open(); cmd.ExecuteNonQuery();
            }
        }
    }
}
