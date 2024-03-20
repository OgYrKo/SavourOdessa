using Microsoft.EntityFrameworkCore;
using NodaTime.Text;


namespace DataLayer.EfClasses;

public partial class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Cityincountry> Cityincountries { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<Dish> Dishes { get; set; }

    public virtual DbSet<Dishbylanguage> Dishbylanguages { get; set; }

    public virtual DbSet<Dishinrestaurant> Dishinrestaurants { get; set; }

    public virtual DbSet<Dishkitchen> Dishkitchens { get; set; }

    public virtual DbSet<Dishorder> Dishorders { get; set; }

    public virtual DbSet<Dishphoto> Dishphotos { get; set; }

    public virtual DbSet<Dishtype> Dishtypes { get; set; }

    public virtual DbSet<Favourite> Favourites { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Openingrule> Openingrules { get; set; }

    public virtual DbSet<Postcode> Postcodes { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Repeatrule> Repeatrules { get; set; }

    public virtual DbSet<Restaurant> Restaurants { get; set; }

    public virtual DbSet<Restaurantphoto> Restaurantphotos { get; set; }

    public virtual DbSet<Restaurantstaff> Restaurantstaffs { get; set; }

    public virtual DbSet<Restauranttable> Restauranttables { get; set; }

    public virtual DbSet<Systemuser> Systemusers { get; set; }

    public virtual DbSet<Tablereservation> Tablereservations { get; set; }

    public virtual DbSet<Userrole> Userroles { get; set; }

    public virtual DbSet<Workhour> Workhours { get; set; }

    [DbFunction("SavourOdessa", "get_group_name")]
    public string get_group_name(string login)
    {
        throw new NotSupportedException();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Cityid).HasName("city_pkey");

            entity.ToTable("city");

            entity.HasIndex(e => e.Cityname, "city_cityname_key").IsUnique();

            entity.Property(e => e.Cityid).HasColumnName("cityid");
            entity.Property(e => e.Cityname)
                .HasColumnType("character varying")
                .HasColumnName("cityname");
        });

        modelBuilder.Entity<Cityincountry>(entity =>
        {
            entity.HasKey(e => e.Cityincountryid).HasName("cityincountry_pkey");

            entity.ToTable("cityincountry");

            entity.HasIndex(e => new { e.Countryid, e.Cityid }, "cityincountry_countryid_cityid_key").IsUnique();

            entity.Property(e => e.Cityincountryid).HasColumnName("cityincountryid");
            entity.Property(e => e.Cityid).HasColumnName("cityid");
            entity.Property(e => e.Countryid).HasColumnName("countryid");

            entity.HasOne(d => d.City).WithMany(p => p.Cityincountries)
                .HasForeignKey(d => d.Cityid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cityincountry_cityid_fkey");

            entity.HasOne(d => d.Country).WithMany(p => p.Cityincountries)
                .HasForeignKey(d => d.Countryid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cityincountry_countryid_fkey");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Commentid).HasName("comment_pkey");

            entity.ToTable("comment");

            entity.HasIndex(e => new { e.Restaurantid, e.Commentdate, e.Userid }, "comment_restaurantid_commentdate_userid_key").IsUnique();

            entity.Property(e => e.Commentid).HasColumnName("commentid");
            entity.Property(e => e.Commentdate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("commentdate");
            entity.Property(e => e.Commenttext).HasColumnName("commenttext");
            entity.Property(e => e.Restaurantid).HasColumnName("restaurantid");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Comments)
                .HasForeignKey(d => d.Restaurantid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comment_restaurantid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comment_userid_fkey");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Countryid).HasName("country_pkey");

            entity.ToTable("country");

            entity.HasIndex(e => e.Countryname, "country_countryname_key").IsUnique();

            entity.Property(e => e.Countryid).HasColumnName("countryid");
            entity.Property(e => e.Countryname)
                .HasColumnType("character varying")
                .HasColumnName("countryname");
            entity.Property(e => e.Currencyid).HasColumnName("currencyid");

            entity.HasOne(d => d.Currency).WithMany(p => p.Countries)
                .HasForeignKey(d => d.Currencyid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("country_currencyid_fkey");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Currencyid).HasName("currency_pkey");

            entity.ToTable("currency");

            entity.HasIndex(e => e.Currencyname, "currency_currencyname_key").IsUnique();

            entity.Property(e => e.Currencyid).HasColumnName("currencyid");
            entity.Property(e => e.Currencyname)
                .HasMaxLength(3)
                .IsFixedLength()
                .HasColumnName("currencyname");
        });

        modelBuilder.Entity<Dish>(entity =>
        {
            entity.HasKey(e => e.Dishid).HasName("dish_pkey");

            entity.ToTable("dish");

            entity.Property(e => e.Dishid).HasColumnName("dishid");
            entity.Property(e => e.Dishkitchenid).HasColumnName("dishkitchenid");
            entity.Property(e => e.Dishtypeid).HasColumnName("dishtypeid");
            entity.Property(e => e.Preparingtime).HasColumnName("preparingtime");

            entity.HasOne(d => d.Dishkitchen).WithMany(p => p.Dishes)
                .HasForeignKey(d => d.Dishkitchenid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dish_dishkitchenid_fkey");

            entity.HasOne(d => d.Dishtype).WithMany(p => p.Dishes)
                .HasForeignKey(d => d.Dishtypeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dish_dishtypeid_fkey");
        });

        modelBuilder.Entity<Dishbylanguage>(entity =>
        {
            entity.HasKey(e => e.Dishbylanguageid).HasName("dishbylanguage_pkey");

            entity.ToTable("dishbylanguage");

            entity.HasIndex(e => new { e.Dishid, e.Languageid }, "dishbylanguage_dishid_languageid_key").IsUnique();

            entity.Property(e => e.Dishbylanguageid).HasColumnName("dishbylanguageid");
            entity.Property(e => e.Dishcomposition).HasColumnName("dishcomposition");
            entity.Property(e => e.Dishid).HasColumnName("dishid");
            entity.Property(e => e.Dishname)
                .HasColumnType("character varying")
                .HasColumnName("dishname");
            entity.Property(e => e.Languageid).HasColumnName("languageid");

            entity.HasOne(d => d.Dish).WithMany(p => p.Dishbylanguages)
                .HasForeignKey(d => d.Dishid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dishbylanguage_dishid_fkey");

            entity.HasOne(d => d.Language).WithMany(p => p.Dishbylanguages)
                .HasForeignKey(d => d.Languageid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dishbylanguage_languageid_fkey");
        });

        modelBuilder.Entity<Dishinrestaurant>(entity =>
        {
            entity.HasKey(e => e.Dishinrestaurantid).HasName("dishinrestaurant_pkey");

            entity.ToTable("dishinrestaurant");

            entity.HasIndex(e => new { e.Restaurantid, e.Dishid }, "dishinrestaurant_restaurantid_dishid_key").IsUnique();

            entity.Property(e => e.Dishinrestaurantid).HasColumnName("dishinrestaurantid");
            entity.Property(e => e.Cost)
                .HasPrecision(8, 2)
                .HasColumnName("cost");
            entity.Property(e => e.Dishid).HasColumnName("dishid");
            entity.Property(e => e.Restaurantid).HasColumnName("restaurantid");

            entity.HasOne(d => d.Dish).WithMany(p => p.Dishinrestaurants)
                .HasForeignKey(d => d.Dishid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dishinrestaurant_dishid_fkey");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Dishinrestaurants)
                .HasForeignKey(d => d.Restaurantid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dishinrestaurant_restaurantid_fkey");
        });

        modelBuilder.Entity<Dishkitchen>(entity =>
        {
            entity.HasKey(e => e.Dishkitchenid).HasName("dishkitchen_pkey");

            entity.ToTable("dishkitchen");

            entity.HasIndex(e => e.Dishkitchenname, "dishkitchen_dishkitchenname_key").IsUnique();

            entity.Property(e => e.Dishkitchenid).HasColumnName("dishkitchenid");
            entity.Property(e => e.Dishkitchenname)
                .HasColumnType("character varying")
                .HasColumnName("dishkitchenname");
        });

        modelBuilder.Entity<Dishorder>(entity =>
        {
            entity.HasKey(e => e.Dishorderid).HasName("dishorder_pkey");

            entity.ToTable("dishorder");

            entity.HasIndex(e => new { e.Dishrestaurantid, e.Tablereservationid }, "dishorder_dishrestaurantid_tablereservationid_key").IsUnique();

            entity.Property(e => e.Dishorderid).HasColumnName("dishorderid");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.Dishrestaurantid).HasColumnName("dishrestaurantid");
            entity.Property(e => e.Tablereservationid).HasColumnName("tablereservationid");
            entity.Property(e => e.Takeawaytime).HasColumnName("takeawaytime");

            entity.HasOne(d => d.Dishrestaurant).WithMany(p => p.Dishorders)
                .HasForeignKey(d => d.Dishrestaurantid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dishorder_dishrestaurantid_fkey");

            entity.HasOne(d => d.Tablereservation).WithMany(p => p.Dishorders)
                .HasForeignKey(d => d.Tablereservationid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dishorder_tablereservationid_fkey");
        });

        modelBuilder.Entity<Dishphoto>(entity =>
        {
            entity.HasKey(e => e.Dishphotoid).HasName("dishphoto_pkey");

            entity.ToTable("dishphoto");

            entity.HasIndex(e => e.Dishphotopath, "dishphoto_dishphotopath_key").IsUnique();

            entity.Property(e => e.Dishphotoid).HasColumnName("dishphotoid");
            entity.Property(e => e.Dishid).HasColumnName("dishid");
            entity.Property(e => e.Dishphotopath)
                .HasColumnType("character varying")
                .HasColumnName("dishphotopath");

            entity.HasOne(d => d.Dish).WithMany(p => p.Dishphotos)
                .HasForeignKey(d => d.Dishid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dishphoto_dishid_fkey");
        });

        modelBuilder.Entity<Dishtype>(entity =>
        {
            entity.HasKey(e => e.Dishtypeid).HasName("dishtype_pkey");

            entity.ToTable("dishtype");

            entity.HasIndex(e => e.Dishtypename, "dishtype_dishtypename_key").IsUnique();

            entity.Property(e => e.Dishtypeid).HasColumnName("dishtypeid");
            entity.Property(e => e.Dishtypename)
                .HasColumnType("character varying")
                .HasColumnName("dishtypename");
        });

        modelBuilder.Entity<Favourite>(entity =>
        {
            entity.HasKey(e => e.Favouriteid).HasName("favourite_pkey");

            entity.ToTable("favourite");

            entity.HasIndex(e => new { e.Restaurantid, e.Userid }, "favourite_restaurantid_userid_key").IsUnique();

            entity.Property(e => e.Favouriteid).HasColumnName("favouriteid");
            entity.Property(e => e.Restaurantid).HasColumnName("restaurantid");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Favourites)
                .HasForeignKey(d => d.Restaurantid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("favourite_restaurantid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Favourites)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("favourite_userid_fkey");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.Languageid).HasName("language_pkey");

            entity.ToTable("language");

            entity.HasIndex(e => e.Languagename, "language_languagename_key").IsUnique();

            entity.Property(e => e.Languageid).HasColumnName("languageid");
            entity.Property(e => e.Languagename)
                .HasColumnType("character varying")
                .HasColumnName("languagename");
        });

        modelBuilder.Entity<Openingrule>(entity =>
        {
            entity.HasKey(e => e.Openingrulesid).HasName("openingrules_pkey");

            entity.ToTable("openingrules");

            entity.HasIndex(e => new { e.Restaurantid, e.Startday, e.Repeatrulesid }, "openingrules_restaurantid_startday_repeatrulesid_key").IsUnique();

            entity.Property(e => e.Openingrulesid).HasColumnName("openingrulesid");
            entity.Property(e => e.Repeatrulesid).HasColumnName("repeatrulesid");
            entity.Property(e => e.Restaurantid).HasColumnName("restaurantid");
            entity.Property(e => e.Startday).HasColumnName("startday");

            entity.HasOne(d => d.Repeatrules).WithMany(p => p.Openingrules)
                .HasForeignKey(d => d.Repeatrulesid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("openingrules_repeatrulesid_fkey");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Openingrules)
                .HasForeignKey(d => d.Restaurantid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("openingrules_restaurantid_fkey");
        });

        modelBuilder.Entity<Postcode>(entity =>
        {
            entity.HasKey(e => e.Postcode1).HasName("postcodes_pkey");

            entity.ToTable("postcodes");

            entity.Property(e => e.Postcode1)
                .HasMaxLength(9)
                .HasColumnName("postcode");
            entity.Property(e => e.Cityincountryid).HasColumnName("cityincountryid");

            entity.HasOne(d => d.Cityincountry).WithMany(p => p.Postcodes)
                .HasForeignKey(d => d.Cityincountryid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("postcodes_cityincountryid_fkey");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.Ratingid).HasName("rating_pkey");

            entity.ToTable("rating");

            entity.HasIndex(e => new { e.Restaurantid, e.Userid }, "rating_restaurantid_userid_key").IsUnique();

            entity.Property(e => e.Ratingid).HasColumnName("ratingid");
            entity.Property(e => e.Rate).HasColumnName("rate");
            entity.Property(e => e.Restaurantid).HasColumnName("restaurantid");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.Restaurantid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rating_restaurantid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rating_userid_fkey");
        });
        var pattern = PeriodPattern.Roundtrip;
        modelBuilder.Entity<Repeatrule>(entity =>
        {
            entity.HasKey(e => e.Repeatrulesid).HasName("repeatrules_pkey");

            entity.ToTable("repeatrules");

            //entity.HasIndex(e => e.Repeatrulesduration, "repeatrules_repeatrulesduration_key").IsUnique();

            entity.HasIndex(e => e.Repeatrulestype, "repeatrules_repeatrulestype_key").IsUnique();

            entity.Property(e => e.Repeatrulesid).HasColumnName("repeatrulesid");
            //entity.Property(e => e.Repeatrulesduration).HasColumnName("repeatrulesduration").HasConversion(
            //    v => pattern.Format(v),
            //    v => pattern.Parse(v).Value
            //);
            entity.Property(e => e.Repeatrulestype)
                .HasColumnType("character varying")
                .HasColumnName("repeatrulestype");
        });

        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(e => e.Restaurantid).HasName("restaurant_pkey");

            entity.ToTable("restaurant");

            entity.HasIndex(e => new { e.Postcode, e.Street, e.Housenum }, "restaurant_postcode_street_housenum_key").IsUnique();

            entity.HasIndex(e => e.Restaurantname, "restaurant_restaurantname_key").IsUnique();

            entity.Property(e => e.Restaurantid).HasColumnName("restaurantid");
            entity.Property(e => e.Housenum)
                .HasMaxLength(10)
                .HasColumnName("housenum");
            entity.Property(e => e.Ownerid).HasColumnName("ownerid");
            entity.Property(e => e.Postcode)
                .HasMaxLength(9)
                .HasColumnName("postcode");
            entity.Property(e => e.Restaurantname)
                .HasColumnType("character varying")
                .HasColumnName("restaurantname");
            entity.Property(e => e.Street)
                .HasColumnType("character varying")
                .HasColumnName("street");
            entity.Property(e => e.Verified)
                .HasDefaultValue(false)
                .HasColumnName("verified");

            entity.HasOne(d => d.Owner).WithMany(p => p.Restaurants)
                .HasForeignKey(d => d.Ownerid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("restaurant_ownerid_fkey");

            entity.HasOne(d => d.PostcodeNavigation).WithMany(p => p.Restaurants)
                .HasForeignKey(d => d.Postcode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("restaurant_postcode_fkey");
        });

        modelBuilder.Entity<Restaurantphoto>(entity =>
        {
            entity.HasKey(e => e.Restaurantphotoid).HasName("restaurantphoto_pkey");

            entity.ToTable("restaurantphoto");

            entity.HasIndex(e => e.Restaurantphotopath, "restaurantphoto_restaurantphotopath_key").IsUnique();

            entity.Property(e => e.Restaurantphotoid).HasColumnName("restaurantphotoid");
            entity.Property(e => e.Restaurantid).HasColumnName("restaurantid");
            entity.Property(e => e.Restaurantphotopath)
                .HasColumnType("character varying")
                .HasColumnName("restaurantphotopath");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Restaurantphotos)
                .HasForeignKey(d => d.Restaurantid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("restaurantphoto_restaurantid_fkey");
        });

        modelBuilder.Entity<Restaurantstaff>(entity =>
        {
            entity.HasKey(e => e.Staffid).HasName("restaurantstaff_pkey");

            entity.ToTable("restaurantstaff");

            entity.HasIndex(e => new { e.Restaurantid, e.Userid }, "restaurantstaff_restaurantid_userid_key").IsUnique();

            entity.Property(e => e.Staffid).HasColumnName("staffid");
            entity.Property(e => e.Restaurantid).HasColumnName("restaurantid");
            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Verified)
                .HasDefaultValue(false)
                .HasColumnName("verified");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Restaurantstaffs)
                .HasForeignKey(d => d.Restaurantid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("restaurantstaff_restaurantid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Restaurantstaffs)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("restaurantstaff_userid_fkey");
        });

        modelBuilder.Entity<Restauranttable>(entity =>
        {
            entity.HasKey(e => e.Tableid).HasName("restauranttable_pkey");

            entity.ToTable("restauranttable");

            entity.Property(e => e.Tableid).HasColumnName("tableid");
            entity.Property(e => e.Restaurantid).HasColumnName("restaurantid");
            entity.Property(e => e.Sitscount).HasColumnName("sitscount");
            entity.Property(e => e.Tablelocation).HasColumnName("tablelocation");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Restauranttables)
                .HasForeignKey(d => d.Restaurantid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("restauranttable_restaurantid_fkey");
        });

        modelBuilder.Entity<Systemuser>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("systemuser_pkey");

            entity.ToTable("systemuser");

            entity.HasIndex(e => e.Username, "systemuser_username_key").IsUnique();

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Username)
                .HasColumnType("character varying")
                .HasColumnName("username");
            entity.Property(e => e.Userroleid).HasColumnName("userroleid");

            entity.HasOne(d => d.Userrole).WithMany(p => p.Systemusers)
                .HasForeignKey(d => d.Userroleid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("systemuser_userroleid_fkey");
        });

        modelBuilder.Entity<Tablereservation>(entity =>
        {
            entity.HasKey(e => e.Tablereservationid).HasName("tablereservation_pkey");

            entity.ToTable("tablereservation");

            entity.Property(e => e.Tablereservationid).HasColumnName("tablereservationid");
            entity.Property(e => e.Duration)
                .HasDefaultValueSql("'01:30:00'::interval")
                .HasColumnName("duration");
            entity.Property(e => e.Reservationtime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("reservationtime");
            entity.Property(e => e.Tableid).HasColumnName("tableid");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Table).WithMany(p => p.Tablereservations)
                .HasForeignKey(d => d.Tableid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tablereservation_tableid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Tablereservations)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tablereservation_userid_fkey");
        });

        modelBuilder.Entity<Userrole>(entity =>
        {
            entity.HasKey(e => e.Userroleid).HasName("userrole_pkey");

            entity.ToTable("userrole");

            entity.HasIndex(e => e.Userrolename, "userrole_userrolename_key").IsUnique();

            entity.Property(e => e.Userroleid).HasColumnName("userroleid");
            entity.Property(e => e.Userrolename)
                .HasColumnType("character varying")
                .HasColumnName("userrolename");
        });

        modelBuilder.Entity<Workhour>(entity =>
        {
            entity.HasKey(e => e.Workhoursid).HasName("workhours_pkey");

            entity.ToTable("workhours");

            entity.HasIndex(e => e.Openingrulesid, "workhours_openingrulesid_key").IsUnique();

            entity.Property(e => e.Workhoursid).HasColumnName("workhoursid");
            entity.Property(e => e.Closehours).HasColumnName("closehours");
            entity.Property(e => e.Openhours).HasColumnName("openhours");
            entity.Property(e => e.Openingrulesid).HasColumnName("openingrulesid");

            entity.HasOne(d => d.Openingrules).WithOne(p => p.Workhour)
                .HasForeignKey<Workhour>(d => d.Openingrulesid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("workhours_openingrulesid_fkey");
        });

        modelBuilder.HasDbFunction(typeof(DataContext).GetMethod(nameof(get_group_name), [typeof(int)]))
            .HasName("get_group_name");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
