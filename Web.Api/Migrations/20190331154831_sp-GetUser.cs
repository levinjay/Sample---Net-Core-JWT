using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Migrations
{
    public partial class spGetUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"SET ANSI_NULLS ON
                GO
                SET QUOTED_IDENTIFIER ON
                GO
                -- =============================================
                -- Author:		Levin jay
                -- Create date: 2019-03-31
                -- Description:	This will return user data
                -- =============================================
                CREATE PROCEDURE [dbo].[spGetUser]
	                -- Add the parameters for the stored procedure here
	                @email nvarchar(max)  
                AS
                BEGIN
	                select * from [dbo].[Users] where Email = @email
                END";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
