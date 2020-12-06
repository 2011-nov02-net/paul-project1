CREATE TABLE [dbo].[customer] (
    [customer_id] INT           IDENTITY (1, 1) NOT NULL,
    [first_name]  NVARCHAR (20) NOT NULL,
    [last_name]   NVARCHAR (20) NOT NULL,
    [date]        DATETIME      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([customer_id] ASC)
);
CREATE TABLE [dbo].[store] (
    [store_id]   INT           IDENTITY (1, 1) NOT NULL,
    [store_name] NVARCHAR (20) NOT NULL,
    [date]       DATETIME      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([store_id] ASC)
);
CREATE TABLE [dbo].[product] (
    [product_id]   INT            IDENTITY (1, 1) NOT NULL,
    [product_name] NVARCHAR (200) NOT NULL,
    [price]        MONEY          NOT NULL,
    [date]         DATETIME       DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([product_id] ASC),
    CHECK ([price]>=(0.0))
);
CREATE TABLE [dbo].[Inventory] (
    [inventory_id] INT IDENTITY (1, 1) NOT NULL,
    [store_id]     INT NOT NULL,
    [product_id]   INT NOT NULL,
    [stock]        INT NOT NULL,
    PRIMARY KEY CLUSTERED ([inventory_id] ASC),
    FOREIGN KEY ([product_id]) REFERENCES [dbo].[product] ([product_id]),
    FOREIGN KEY ([store_id]) REFERENCES [dbo].[store] ([store_id]),
    CHECK ([stock]>=(0))
);
CREATE TABLE [dbo].[order] (
    [order_id]         INT      IDENTITY (1, 1) NOT NULL,
    [customer_id]      INT      NOT NULL,
    [store_id]         INT      NOT NULL,
    [order_total] MONEY    NOT NULL,
    [date]             DATETIME DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([order_id] ASC),
    FOREIGN KEY ([customer_id]) REFERENCES [dbo].[customer] ([customer_id]),
    FOREIGN KEY ([store_id]) REFERENCES [dbo].[store] ([store_id]),
    CHECK ([order_total]>=(0))
);
CREATE TABLE [dbo].[order_items] (
    [item_id]    INT   IDENTITY (1, 1) NOT NULL,
    [order_id]   INT   NOT NULL,
    [product_id] INT   NOT NULL,
    [quantity]   INT   NOT NULL,
    [total]      MONEY NULL,
    PRIMARY KEY CLUSTERED ([item_id] ASC),
    FOREIGN KEY ([product_id]) REFERENCES [dbo].[product] ([product_id]),
    FOREIGN KEY ([order_id]) REFERENCES [dbo].[order] ([order_id]),
    CHECK ([total]>=(0))
);