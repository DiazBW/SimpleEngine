$(document).ready(function () {
    
    var modelInitialData = {
        PlayerOneId: "11",
        PlayerTwoId: "13",
        Rows: 
        [
            {
                RowIndex : 0,
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
                    }
                ]
            },
            {
                RowIndex : 1,
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
                    }
                ]
            },
            {
                RowIndex : 2,
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
                    }
                ]
            }
        ]
    };

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
    }

    var viewModel = new BoardViewModel(modelInitialData.Rows, modelInitialData.PlayerOneId, modelInitialData.PlayerTwoId);
    ko.mapping.fromJS(modelJson, {}, viewModel);
    ko.applyBindings(viewModel);
});

