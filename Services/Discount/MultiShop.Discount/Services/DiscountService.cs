using Dapper;
using MultiShop.Discount.Context;
using MultiShop.Discount.Dtos;

namespace MultiShop.Discount.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly DapperContext _context;

        // Constructor: DapperContext nesnesi dependency injection ile alınır
        public DiscountService(DapperContext context)
        {
            _context = context;
        }

        // Yeni bir indirim kuponu oluşturur ve veritabanına ekler
        public async Task CreateDiscountCouponAsync(CreateDiscountCouponDto createCouponDto)
        {
            // SQL sorgusu: Kupon bilgilerini Coupons tablosuna ekle
            const string query = @"
                INSERT INTO Coupons (Code, Rate, IsActive, ValidDate) 
                VALUES (@Code, @Rate, @IsActive, @ValidDate)";

            // Dapper parametreleri oluşturuluyor
            var parameters = new DynamicParameters();
            parameters.Add("Code", createCouponDto.Code);
            parameters.Add("Rate", createCouponDto.Rate);
            parameters.Add("IsActive", createCouponDto.IsActive);
            parameters.Add("ValidDate", createCouponDto.ValidDate);

            // Veritabanı bağlantısı oluşturulur ve sorgu çalıştırılır
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        // Verilen ID'ye göre indirim kuponunu siler
        public async Task DeleteDiscountCouponAsync(int id)
        {
            // SQL sorgusu: Belirtilen CouponId'ye sahip kaydı sil
            const string query = "DELETE FROM Coupons WHERE CouponId = @Id";

            // Parametreyi oluştur
            var parameters = new DynamicParameters();
            parameters.Add("Id", id);

            // Sorguyu çalıştır
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        // Tüm indirim kuponlarını getirir
        public async Task<List<ResultDiscountCouponDto>> GetAllDiscountCouponsAsync()
        {
            // SQL sorgusu: Tüm kayıtları getir
            const string query = "SELECT * FROM Coupons";

            // Bağlantıyı aç, sorguyu çalıştır, sonuçları liste olarak döndür
            using (var connection = _context.CreateConnection())
            {
                var coupons = await connection.QueryAsync<ResultDiscountCouponDto>(query);
                return coupons.ToList();
            }
        }

        // Belirtilen ID'ye sahip kuponu getirir
        public async Task<GetByIdDiscountCouponDto> GetByIdDiscountCouponAsync(int id)
        {
            // SQL sorgusu: ID'ye göre kayıt getir
            const string query = "SELECT * FROM Coupons WHERE CouponId = @Id";

            // Parametreyi oluştur
            var parameters = new DynamicParameters();
            parameters.Add("Id", id);

            // Sorguyu çalıştır ve tek bir kayıt döndür
            using (var connection = _context.CreateConnection())
            {
                var coupon = await connection.QuerySingleOrDefaultAsync<GetByIdDiscountCouponDto>(query, parameters);
                return coupon;
            }
        }

        // Var olan bir kuponu günceller
        public async Task UpdateDiscountCouponAsync(UpdateDiscountCouponDto updateCouponDto)
        {
            // SQL sorgusu: Kupon bilgilerini güncelle
            const string query = @"
                UPDATE Coupons 
                SET Code = @Code, 
                    Rate = @Rate, 
                    IsActive = @IsActive, 
                    ValidDate = @ValidDate 
                WHERE CouponId = @CouponId";

            // Güncellenecek alanlar için parametreleri hazırla
            var parameters = new DynamicParameters();
            parameters.Add("CouponId", updateCouponDto.CouponId);
            parameters.Add("Code", updateCouponDto.Code);
            parameters.Add("Rate", updateCouponDto.Rate);
            parameters.Add("IsActive", updateCouponDto.IsActive);
            parameters.Add("ValidDate", updateCouponDto.ValidDate);

            // Veritabanı bağlantısı oluşturulur ve sorgu çalıştırılır
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }
    }
}