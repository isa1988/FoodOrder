# FoodOrder [API schema]

**Auth**

[POST] [] Account/register (int Id, string Name, string Email, string Password, string ConfirmPassword) => ()

[POST] [] Account/login (string Email, string Password) => ()

**Organizations**

[GET] [Authorize(Roles = "User")] Organization () => ()

[GET] [] Organization () => ()

[GET] [Authorize(Roles = "Admin")] Organization () => ()

[POST] [Authorize(Roles = "Admin")] Organization/Create (string Name, string PictureName, string PictureFormat, byte[] File, string Comment) => ()

[POST] [Authorize(Roles = "Admin")] Organization/Edit/id (int Id, string Name, string PictureName, string PictureFormat, byte[] File, IFormFile WorkToFile, bool IsPictureDelete, string Comment) => ()

[POST] [Authorize(Roles = "Admin")] Organization/Delete/id (string Name, string PictureName, string PictureFormat, string Comment) => ()

**Dish** 

[GET] [Authorize(Roles = "Admin")] Dish/Get/OrganizationId () => ()

[GET] [] Dish/Get/OrganizationId () => ()

[POST] [Authorize(Roles = "Admin")] Dish/Create (string Name, int OrganizationId, SelectList OrganizationList, decimal Price, string PictureName, string PictureFormat, byte[] File, IFormFile WorkToFile, bool IsPictureDelete, string Comment) => ()

[POST] [Authorize(Roles = "Admin")] Dishes/Edit (string Name, int OrganizationId, SelectList OrganizationList, decimal Price, string PictureName, string PictureFormat, byte[] File, IFormFile WorkToFile, string Comment) => ()

[POST] [Authorize(Roles = "Admin")] Dishes/Delete (int Id, string Name, int OrganizationId, string Organization, decimal Price, string PictureName, string PictureFormat, string Comment) => ()

**Basket**

[GET] [Authorize(Roles = "User")] Busket () => ()

[POST] [Authorize(Roles = "User")] Busket/Pay/id (int Id, int UserId, int OrganizationId, List<BasketInventoryEditDto> BasketInventoryEditModels, decimal Sum) => ()

[POST] [Authorize(Roles = "User")] Busket/CreateOrEdit/id (int Id, int UserId, int OrganizationId, List<BasketInventoryRatingAndComment> BasketInventoryRatingAndComments) => ()

[POST] [Authorize(Roles = "User")] Busket/RatingAndComment/id (int Id, int UserId, int OrganizationId, List<BasketInventoryRatingAndComment> BasketInventoryRatingAndComments) => ()

[POST] [Authorize(Roles = "User")] Busket/Delete/id (int Id, int UserId, int OrganizationId, List<BasketInventoryEditDto> BasketInventoryEditModels, decimal Sum) => ()

