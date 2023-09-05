using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeskManager.Migrations
{
    /// <inheritdoc />
    public partial class AddIsAvailableTriger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        CREATE TRIGGER UpdateIsAvailableOnDesk
        ON Desks
        AFTER INSERT, UPDATE
        AS
        BEGIN
            UPDATE Desks
            SET IsAvailable = CASE
                 WHEN GETDATE() >= Desks.StartReservation AND GETDATE() <= Desks.EndReservation THEN 0
                ELSE 1 
            END
            FROM inserted
            WHERE Desks.Id = inserted.Id;
        END;
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER UpdateIsAvailableOnDesk;");
        }

    }
}
