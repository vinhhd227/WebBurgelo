using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBurgelo.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Delivery",
                columns: table => new
                {
                    DeliveryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShipperId = table.Column<int>(type: "int", nullable: false),
                    DeliveryStatus = table.Column<int>(type: "int", nullable: false),
                    CustomerConfirm = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delivery", x => x.DeliveryId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Subscribe",
                columns: table => new
                {
                    SubscribeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscribe", x => x.SubscribeId);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Product_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DeliveryId = table.Column<int>(type: "int", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubTotal = table.Column<int>(type: "int", nullable: false),
                    ConfirmStatus = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Order_Delivery_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "Delivery",
                        principalColumn: "DeliveryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateTable(
                name: "Order Detail",
                columns: table => new
                {
                    OrderDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order Detail", x => x.OrderDetailId);
                    table.ForeignKey(
                        name: "FK_Order Detail_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order Detail_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Account_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VerifyEmail",
                columns: table => new
                {
                    VerifyEmailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    ExpirationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerifyEmail", x => x.VerifyEmailId);
                    table.ForeignKey(
                        name: "FK_VerifyEmail_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.InsertData(
                            table: "Category",
                            columns: new[] { "CategoryId", "CategoryName" },
                            values: new object[] {
                    1,
                    "Burgers"
                            }
                        );
            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[] {
                    2,
                    "Pizza"
                }
            );
            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[] {
                    3,
                    "Drinks"
                }
            );
            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[] {
                    4,
                    "Pasta"
                }
            );
            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[] {
                    5,
                    "Soup"
                }
            );
            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[] {
                    6,
                    "Sushi"
                }
            );
            // ADD Product
            migrationBuilder.InsertData(
                         table: "Product",
                         columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                         values: new object[] {
                    "BBQ Burger",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    1
                         }
                     );
            migrationBuilder.InsertData(
                        table: "Product",
                        columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                        values: new object[] {
                    "Black Burger",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    1
                        }
                    );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Chicken Burger",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    1
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Smoked Meat Burger",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    1
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Teriyaki Chicken Burger",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    1
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Turkey Burger",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    1
                }
            );
            migrationBuilder.InsertData(
                         table: "Product",
                         columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                         values: new object[] {
                    "Beacon Burger",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    1
                         }
                     );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Texas Burger",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    1
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Burger Combo",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    1
                }
            );
            migrationBuilder.InsertData(
      table: "Product",
      columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
      values: new object[] {
                    "Beacon Pizza",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    2
      }
  );
            migrationBuilder.InsertData(
               table: "Product",
               columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
               values: new object[] {
                    "Kebab Pizza",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    2
               }
           );
            migrationBuilder.InsertData(
               table: "Product",
               columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
               values: new object[] {
                    "Margherita Pizza",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    2
               }
           );
            migrationBuilder.InsertData(
               table: "Product",
               columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
               values: new object[] {
                    "Mushroom Pizza",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    2
               }
           );
            migrationBuilder.InsertData(
               table: "Product",
               columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
               values: new object[] {
                    "Sausage Pizza",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    2
               }
           );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Tomato Pizza",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    2
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Sea Food Pizza",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    2
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Pepperoni Pizza",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    2
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Vegetable Pizza",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    2
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Banana Juice",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    3
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Orange Juice",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    3
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Coffe",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    3
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Coke",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    3
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Carrot Juice",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    3
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Lemon Tea",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    3
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Smoothie Milkshake",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    3
                }
            );
            migrationBuilder.InsertData(
              table: "Product",
              columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
              values: new object[] {
                    "Hot Cacao",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    3
              }
          );
            migrationBuilder.InsertData(
               table: "Product",
               columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
               values: new object[] {
                    "Blue Curacao Soda",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    3
               }
           );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Pasta Neo",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    4
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Beacon Pasta",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    4
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Bologness Pasta",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    4
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Chicken Pasta",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    4
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Chinese Noodles Pasta",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    4
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Meat Spaghetti",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    4
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Phomai Pasta",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    4
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Salad Pasta",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    4
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Sea Food Noodles",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    4
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Tomato Pasta",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    4
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Season Soup",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    5
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Chicken Soup",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    5
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Crab Soup",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    5
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Green Thai Chicken",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    5
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Hangover Soup",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    5
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Hot Soup",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    5
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Meat Soup",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    5
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Pumpkin Soup",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    5
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Ratatouille Soup",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    5
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Wellness Soup",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    5
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Uramaki",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    6
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Califonia Maki",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    6
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Sushi Combo",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    6
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Temaki",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    6
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Sashimi",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    6
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Sake",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    6
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Masago",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    6
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Fried Umaki",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    6
                }
            );
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductName", "Description", "Price", "CategoryId" },
                values: new object[] {
                    "Philadelphia Maki",
                    "These BBQ Burgers are an amazingly juicy explosion of sweet, tangy, spicy, smokey, crispy deliciousness all at the same time. They’re smothered in homemade BBQ sauce, sharp cheddar, salty bacon, crispy lettuce, juicy tomatoes and crispy onion strings.  Of course, all of the toppings are customizable to make it YOUR best burger recipe! I’ve included tips and tricks on how to cook burgers (grilled or stove top), how to make ahead and how to freeze and my secret to the JUICIEST burgers!",
                    15,
                    6
                }
            );




            migrationBuilder.InsertData(
                   table: "Role",
                   columns: new[] { "RoleId", "RoleName" },
                   values: new object[] {
                    1,
                    "Unverified"
                   }
               );
            migrationBuilder.InsertData(
                    table: "Role",
                    columns: new[] { "RoleId", "RoleName" },
                    values: new object[] {
                    2,
                    "Customer"
                    }
                );
            migrationBuilder.InsertData(
               table: "Role",
               columns: new[] { "RoleId", "RoleName" },
               values: new object[] {
                    3,
                    "Employee"
               }
           );
            migrationBuilder.InsertData(
               table: "Role",
               columns: new[] { "RoleId", "RoleName" },
               values: new object[] {
                    4,
                    "Admin"
               }
           );
            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "UserName", "Gender", "DateOfBirth", "Email", "RoleId", "Address", "PhoneNumber" },
                values: new object[] {
                    1,
                    "Admin",
                    "Male",
                    DateTime.Now,
                    "admin@gmail.com",
                    4,
                    "Le Duc Tho",
                    "0373431221"
                }
            );
            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "AccountId", "AccountName", "Password", "UserId" },
                values: new object[] {
                    1,
                    "admin",
                    "admin",
                    1
                }
            );
            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "UserName", "Gender", "DateOfBirth", "Email", "RoleId", "Address", "PhoneNumber" },
                values: new object[] {
                    2,
                    "Vinh Hoang 1",
                    "Male",
                    DateTime.Now,
                    "vinhhd227@gmail.com",
                    1,
                    "Le Duc Tho 1",
                    "0373431227"
                }
            );
            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "AccountId", "AccountName", "Password", "UserId" },
                values: new object[] {
                    2,
                    "vinh1",
                    "123",
                    2
                }
            );
            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "UserName", "Gender", "DateOfBirth", "Email", "RoleId", "Address", "PhoneNumber" },
                values: new object[] {
                    3,
                    "Vinh Hoang 2",
                    "Male",
                    DateTime.Now,
                    "vinh2@gmail.com",
                    2,
                    "Le Duc Tho 2",
                    "0373431228"
                }
            );
            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "AccountId", "AccountName", "Password", "UserId" },
                values: new object[] {
                    3,
                    "vinh2",
                    "123",
                    3
                }
            );
            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "UserName", "Gender", "DateOfBirth", "Email", "RoleId", "Address", "PhoneNumber" },
                values: new object[] {
                    4,
                    "Vinh Hoang 3",
                    "Male",
                    DateTime.Now,
                    "vinh3@gmail.com",
                    3,
                    "Le Duc Tho 3",
                    "0373431229"
                }
            );
            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "AccountId", "AccountName", "Password", "UserId" },
                values: new object[] {
                    4,
                    "vinh3",
                    "123",
                    4
                }
            );
            migrationBuilder.InsertData(
          table: "Subscribe",
          columns: new[] { "Email" },
          values: new object[] {
                    "vinhhd227@gmail.com"
                }
            );
            migrationBuilder.InsertData(
          table: "Delivery",
          columns: new[] { "DeliveryId", "DeliveryCode", "ShipperId", "DeliveryStatus", "CustomerConfirm", },
          values: new object[] {
                    1,
                    "QDAA3IOTQ",
                    0,
                    0,
                    0
                }
            );
            migrationBuilder.InsertData(
         table: "Delivery",
         columns: new[] { "DeliveryId", "DeliveryCode", "ShipperId", "DeliveryStatus", "CustomerConfirm" },
         values: new object[] {
                    2,
                    "QDAA4IOTQ",
                    0,
                    0,
                    0
               }
           );
            migrationBuilder.InsertData(
          table: "Order",
          columns: new[] { "OrderId", "OrderCode", "UserId", "DeliveryId", "CustomerName", "Phone", "Address", "OrderDateTime", "SubTotal", "ConfirmStatus", "PaymentMethod", "PaymentStatus" },
          values: new object[] {
                    1,
                    "QDAA3IOTQ",
                    1,
                    1,
                    "Hoàng Đông Vĩnh",
                    "0373431227",
                    "CT4 Lê ĐỨc Thọ",
                    DateTime.Today.AddDays(-8),
                    90.00,
                    0,
                    1,
                    0
                }
            );

            migrationBuilder.InsertData(
 table: "Order Detail",
 columns: new[] { "OrderDetailId", "OrderId", "ProductId", "Quantity" },
 values: new object[] {
                    1,
                    1,
                    1,
                    1
       }
   );
            migrationBuilder.InsertData(
                   table: "Order",
                   columns: new[] { "OrderId", "OrderCode", "UserId", "DeliveryId", "CustomerName", "Phone", "Address", "OrderDateTime", "SubTotal", "ConfirmStatus", "PaymentMethod", "PaymentStatus" },
                   values: new object[] {
                    2,
                    "QDAA4IOTQ",
                    1,
                    1,
                    "Hoàng Đông Vĩnh",
                    "0373431227",
                    "CT4 Lê ĐỨc Thọ",
                    DateTime.Now,
                    90.00,
                    0,
                    1,
                    0
                         }
                     );

            migrationBuilder.InsertData(
 table: "Order Detail",
 columns: new[] { "OrderDetailId", "OrderId", "ProductId", "Quantity" },
 values: new object[] {
                    4,
                    2,
                    1,
                    5
       }
   );
            migrationBuilder.InsertData(
         table: "Order Detail",
         columns: new[] { "OrderDetailId", "OrderId", "ProductId", "Quantity" },
         values: new object[] {
                    2,
                    1,
                    7,
                    2
               }
           );
            migrationBuilder.InsertData(
         table: "Order Detail",
         columns: new[] { "OrderDetailId", "OrderId", "ProductId", "Quantity" },
         values: new object[] {
                    3,
                    1,
                    18,
                    3
               }
           );
            migrationBuilder.CreateIndex(
                name: "IX_Account_UserId",
                table: "Account",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_DeliveryId",
                table: "Order",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_Order Detail_OrderId",
                table: "Order Detail",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Order Detail_ProductId",
                table: "Order Detail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_VerifyEmail_UserId",
                table: "VerifyEmail",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Order Detail");

            migrationBuilder.DropTable(
                name: "Subscribe");

            migrationBuilder.DropTable(
                name: "VerifyEmail");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Delivery");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
