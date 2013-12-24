$(document).ready(function () {
    var modelJson = {
        UserOneId: "11",
        UserTwoId: "13",
        ActiveUserId: "11",
        Cells: {
            Rows : 
            [
                { RowIndex: 0, Columns: [1, 2, 0] },
                { RowIndex: 1, Columns: [0, 2, 1] },
                { RowIndex: 2, Columns: [2, 0, 0] }
            ],
        }
    };
    
    function CellViewModel(value, rowIndex, columnIndex) {
        var self = this;
        self.Value = ko.observable(value);
        self.RowIndex = ko.observable(rowIndex);
        self.ColumnIndex = ko.observable(columnIndex);
    }

    function RowViewModel(rowInitialData, rowIndex) {
        var self = this;
        self.Cells = ko.observableArray(ko.utils.arrayMap(rowInitialData, function (cellData) {
            return new CellViewModel(cellData.Value, cellData.RowIndex, cellData.ColumnIndex);
        })),
        self.RowIndex = ko.observable(rowIndex);
    }

    function BoardViewModel(rowsInitalData, playerOneId, playerTwoId) {
        var self = this;
        self.UserOneId = ko.observable(playerOneId);
        self.UserTwoId = ko.observable(playerTwoId);
        self.ActiveUserId = ko.observable(playerOneId);
        self.Rows = ko.observableArray(ko.utils.arrayMap(rowsInitalData, function (rowData) {
            return new RowViewModel(rowData.Cells, rowData.RowIndex);
        })),
        
        self.ActiveCellType = ko.computed(function() {
            var activeCellType = "Empty";
            if (self.ActiveUserId() == self.UserOneId()) {
                activeCellType = "Black";
            } else {
                activeCellType = "White";
            }
                
            return activeCellType;
        });

        self.CellClicked = function (cellViewModel) {
            var typeValue = cellViewModel.Value();
            var newValue = typeValue;
            if (typeValue == 0) {
                newValue = 1;
            } else if (typeValue == 1) {
                newValue = 2;
            } else if (typeValue == 2) {
                newValue = 0;
            }
            cellViewModel.Value(newValue);
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
        
        self.SaveGame = function () {
            //$.getJSON("/Test/AjaxSave/")
            //    .done(function() {
            //        console.log("Save ajax done!");
            //    })
            //    .fail(function() {
            //        console.log("Save ajax failed!");
            //    });
            
            $.post("/Test/AjaxSave/")
            .done(function(data) {
                console.log("Save ajax done!");
            })
            .fail(function() {
                console.log("Save ajax failed!");
            });
        };
        
        self.LoadGame = function () {
            //$.getJSON("/Test/AjaxLoad/")
            //    .done(function () {
            //        console.log("Load ajax done!");
            //    })
            //    .fail(function () {
            //        console.log("Save ajax failed!");
            //    });
            
            //var jsonData = ko.toJSON(viewModel);
            //var plainJs = ko.toJS(viewModel);

            

            $.post("/Test/AjaxLoad/")
            .done(function (data) {
                var someJSON = data;
                var parsed = JSON.parse(someJSON);
 
                // Update view model properties
                //self..firstName(parsed.firstName);
                //viewModel.pets(parsed.pets);
                
                //ko.mapping.fromJS(someJSON, {}, self);
                self = ko.mapping.fromJS(parsed);

                console.log("Load ajax done!");
            })
            .fail(function () {
                console.log("Save ajax failed!");
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

    var viewModel = new BoardViewModel(modelInitialData.Rows, modelInitialData.PlayerOneId, modelInitialData.PlayerTwoId);
    ko.mapping.fromJS(modelJson, {}, viewModel);
    ko.applyBindings(viewModel);
});

