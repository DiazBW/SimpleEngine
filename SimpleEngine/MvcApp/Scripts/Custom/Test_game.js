$(document).ready(function () {
    var modelJson = {
        PlayerOneId: "1",
        PlayerTwoId: "2",
        ActiveUserId: "1",
        Cells: {
            Rows : 
            [
                { RowIndex: 0, Columns: [1, 2, 0] },
                { RowIndex: 1, Columns: [0, 2, 1] },
                { RowIndex: 2, Columns: [2, 0, 0] }
            ],
        }
    };
    
    // maybe switch to ko.mapping
    function CellViewModel(value, rowIndex, columnIndex) {
        var self = this;
        self.Value = ko.observable(value);
        self.RowIndex = ko.observable(rowIndex);
        self.ColumnIndex = ko.observable(columnIndex);

        self.GetJS = function() {
            return {
                Value: self.Value(),
                RowIndex: self.RowIndex(),
                ColumnIndex: self.ColumnIndex()
            };
        };
    }

    function RowViewModel(cellObjectList, rowIndex) {
        var self = this;
        self.Cells = ko.observableArray(ko.utils.arrayMap(cellObjectList, function (cellData) {
            return new CellViewModel(cellData.Value, cellData.RowIndex, cellData.ColumnIndex);
        })),
        self.RowIndex = ko.observable(rowIndex);
        
        self.GetJS = function () {
            var cells = [];
            for (var index in self.Cells()) {
                var cellJs = self.Cells()[index].GetJS();
                cells.push(cellJs);
            }
            return {
                Cells: cells,
                RowIndex: self.RowIndex()
            };
        };
    }

    function BoardViewModel(rowsInitalData, playerOneId, playerTwoId, gameId) {
        var self = this;
        self.defData = rowsInitalData;
        self.GameId = ko.observable(gameId);
        self.PlayerOneId = ko.observable(playerOneId);
        self.PlayerTwoId = ko.observable(playerTwoId);
        self.ActiveUserId = ko.observable(playerOneId);
        self.Rows = ko.observableArray(ko.utils.arrayMap(rowsInitalData, function (rowData) {
            return new RowViewModel(rowData.Cells, rowData.RowIndex);
        })),
        
        self.ActiveCellType = ko.computed(function() {
            var activeCellType = "Empty";
            if (self.ActiveUserId() == self.PlayerOneId()) {
                activeCellType = "Black";
            } else {
                activeCellType = "White";
            }
                
            return activeCellType;
        });

        self.CellClicked = function (cellViewModel) {
            self.SendTurn(cellViewModel.RowIndex(), cellViewModel.ColumnIndex());
        };

        self.GetClassForCell = function (cellViewModel) {
            var cellTypeId = cellViewModel.Value();
            var typeText = "empty";
            if (cellTypeId == 1) {
                typeText = "black";
            }
            if (cellTypeId == 2) {
                typeText = "white";
            }
            return "cell-type-" + typeText;
        };
        
        //self.SaveGame = function () {
        //    var unmappedModel = self.GetJS();
        //    var jsonForSave = JSON.stringify(unmappedModel, null, 2);
            
        //    $.ajax({
        //        url: '/Test/AjaxSave',
        //        type: 'POST',
        //        dataType: 'json',
        //        data: jsonForSave,
        //        contentType: 'application/json; charset=utf-8',
        //        success: function (data) {
        //            // TODO: error handling
        //            console.log(data);
        //        },
        //        error: function (data) {
        //            console.log(data);
        //        }
        //    });
        //};

        //self.LoadGame = function () {
        //    $.post("/Test/AjaxLoad/")
        //    .done(function (jsonData) {
        //        var jsData = JSON.parse(jsonData);

        //        self.PlayerOneId(jsData.PlayerOneId);
        //        self.PlayerTwoId(jsData.PlayerTwoId);
        //        self.ActiveUserId(jsData.ActivePlayerId);
                
        //        self.Rows.removeAll();
        //        for (var index in jsData.Rows) {
        //            var cells = jsData.Rows[index].Cells;
        //            var rowIndex = jsData.Rows[index].RowIndex;
        //            self.Rows.push(new RowViewModel(cells, rowIndex));
        //        }
        //        console.log("Load ajax done!");
        //    })
        //    .fail(function () {
        //        console.log("Save ajax failed!");
        //    });
        //};

        self.GetJS = function () {
            var rows = [];
            for (var index in self.Rows()) {
                var rowJs = self.Rows()[index].GetJS();
                rows.push(rowJs);
            }
            return {
                Rows: rows,
                PlayerOneId: self.PlayerOneId(),
                PlayerTwoId: self.PlayerTwoId(),
                ActiveUserId: self.ActiveUserId()
            };
        };
        
        self.SkipTurn = function () {
            var postData = { gameId: self.GameId() };
            var jsonData = JSON.stringify(postData, null, 2);
            $.ajax({
                url: '/Game/SkipTurn',
                type: 'POST',
                dataType: 'json',
                data: jsonData,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    // TODO: error handling
                    alert("turnSkipped " + data);
                    console.log(data);
                },
                error: function (data) {
                    console.log(data);
                }
            });
        };
        
        self.Surrender = function () {
            var postData = { gameId: self.GameId() };
            var jsonData = JSON.stringify(postData, null, 2);
            $.ajax({
                url: '/Game/Surrender',
                type: 'POST',
                dataType: 'json',
                data: jsonData,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    // TODO: error handling
                    alert("you`ve surrendered " + data);
                    console.log(data);
                },
                error: function (data) {
                    console.log(data);
                }
            });
        };

        self.OpenGame = function () {
            var postData = { playerId: self.PlayerOneId() };
            var jsonData = JSON.stringify(postData, null, 2);
            $.ajax({
                url: '/Game/OpenGame',
                type: 'POST',
                dataType: 'json',
                data: jsonData,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    // TODO: error handling
                    alert("gameOpened " + data.NewGameId);
                    console.log(data);
                },
                error: function (data) {
                    console.log(data);
                }
            });
        };
        
        self.CloseGame = function () {
            // BUG: self.GameId - gameRequestId - create single page - for a while its bug
            var postData = { playerId: self.PlayerTwoId(), gameRequestId: self.GameId() };
            var jsonData = JSON.stringify(postData, null, 2);
            $.ajax({
                url: '/Game/CloseGame',
                type: 'POST',
                dataType: 'json',
                data: jsonData,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    // TODO: error handling
                    alert("gameClosed " + data);
                    console.log(data);
                },
                error: function (data) {
                    console.log(data);
                }
            });
        };
        
        self.GetGame = function () {
            var postData = { gameId: self.GameId() };
            var jsonData = JSON.stringify(postData, null, 2);
            $.ajax({
                url: '/Game/GetGame',
                type: 'POST',
                dataType: 'json',
                data: jsonData,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    // TODO: error handling
                    console.log(data);
                    
                    var jsData = JSON.parse(data);

                    self.GameId(jsData.GameId);
                    self.PlayerOneId(jsData.PlayerOneId);
                    self.PlayerTwoId(jsData.PlayerTwoId);
                    self.ActiveUserId(jsData.ActivePlayerId);

                    self.Rows.removeAll();
                    for (var index in jsData.Rows) {
                        var cells = jsData.Rows[index].Cells;
                        var rowIndex = jsData.Rows[index].RowIndex;
                        self.Rows.push(new RowViewModel(cells, rowIndex));
                    }
                },
                error: function (data) {
                    alert("get game error");
                    console.log(data);
                }
            });
        };
        
        self.SendTurn = function (rowIndex, columnIndex) {
            var postData = {
                RowIndex: rowIndex,
                ColumnIndex: columnIndex,
                GameId: self.GameId()
            };
            var jsonData = JSON.stringify(postData, null, 2);
            $.ajax({
                url: '/Game/Turn',
                type: 'POST',
                dataType: 'json',
                data: jsonData,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    // TODO: error handling
                    console.log(data);
                    self.GetGame();
                },
                error: function (data) {
                    alert("turn error");
                    console.log(data);
                }
            });
        };
    }

    var modelInitialData = {
        PlayerOneId: "11",
        PlayerTwoId: "13",
        Rows:
        [
            {
                RowIndex: 0,
                Cells:
                [
                    {
                        RowIndex: 0,
                        ColumnIndex: 0,
                        Value: 0
                    },
                    {
                        RowIndex: 0,
                        ColumnIndex: 1,
                        Value: 0
                    },
                    {
                        RowIndex: 0,
                        ColumnIndex: 2,
                        Value: 0
                    },
                    {
                        RowIndex: 0,
                        ColumnIndex: 3,
                        Value: 0
                    },
                    {
                        RowIndex: 0,
                        ColumnIndex: 4,
                        Value: 0
                    }
                ]
            },
            {
                RowIndex: 1,
                Cells:
                [
                    {
                        RowIndex: 1,
                        ColumnIndex: 0,
                        Value: 0
                    },
                    {
                        RowIndex: 1,
                        ColumnIndex: 1,
                        Value: 0
                    },
                    {
                        RowIndex: 1,
                        ColumnIndex: 2,
                        Value: 0
                    },
                    {
                        RowIndex: 1,
                        ColumnIndex: 3,
                        Value: 0
                    },
                    {
                        RowIndex: 1,
                        ColumnIndex: 4,
                        Value: 0
                    }
                ]
            },
            {
                RowIndex: 2,
                Cells:
                [
                    {
                        RowIndex: 2,
                        ColumnIndex: 0,
                        Value: 0
                    },
                    {
                        RowIndex: 2,
                        ColumnIndex: 1,
                        Value: 0
                    },
                    {
                        RowIndex: 2,
                        ColumnIndex: 2,
                        Value: 0
                    },
                    {
                        RowIndex: 2,
                        ColumnIndex: 3,
                        Value: 0
                    },
                    {
                        RowIndex: 2,
                        ColumnIndex: 4,
                        Value: 0
                    }
                ]
            },
            {
                RowIndex: 3,
                Cells:
                [
                    {
                        RowIndex: 3,
                        ColumnIndex: 0,
                        Value: 0
                    },
                    {
                        RowIndex: 3,
                        ColumnIndex: 1,
                        Value: 0
                    },
                    {
                        RowIndex: 3,
                        ColumnIndex: 2,
                        Value: 0
                    },
                    {
                        RowIndex: 3,
                        ColumnIndex: 3,
                        Value: 0
                    },
                    {
                        RowIndex: 3,
                        ColumnIndex: 4,
                        Value: 0
                    }
                ]
            },
            {
                RowIndex: 4,
                Cells:
                [
                    {
                        RowIndex: 4,
                        ColumnIndex: 0,
                        Value: 0
                    },
                    {
                        RowIndex: 4,
                        ColumnIndex: 1,
                        Value: 0
                    },
                    {
                        RowIndex: 4,
                        ColumnIndex: 2,
                        Value: 0
                    },
                    {
                        RowIndex: 4,
                        ColumnIndex: 3,
                        Value: 0
                    },
                    {
                        RowIndex: 4,
                        ColumnIndex: 4,
                        Value: 0
                    }
                ]
            }
        ]
    };

    var viewModel = new BoardViewModel(modelInitialData.Rows, modelInitialData.PlayerOneId, modelInitialData.PlayerTwoId, 3);
    ko.mapping.fromJS(modelJson, {}, viewModel);
    ko.applyBindings(viewModel);
});

