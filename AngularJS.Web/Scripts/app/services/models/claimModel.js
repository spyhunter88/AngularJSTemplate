'use strict';

app.factory('claimModel', function() {
	return kendo.data.Model.define({
		id: "ClaimID",
		fields: {
			ClaimID: {type: "number", title: "ID" },
			Status: {type: "number", title: "Status" },
			ClaimType: {type: "string", title: "Type" },
			BuName: { type: "string", title: "BU" },
			ParticipantClaim: { type: "string", title: "Participant" },
			UnitClaim: { type: "string", title: "Unit" },
			FTGProgramCode: { type: "string", title: "Program Code" },
			VendorProgramCode: { type: "string", title:"Vendor Program Code" },
			ProgramName: { type: "string", title: "Program Code" },
			ProgramType: { type: "string", title: "Program Type" },
			VendorName: { type: "string", title: "Vendor" },
			RemainPayment: { type: "number", title: "Remain Payment" },
			RemainAllocation: { type: "number", title: "Remain Allocation" },
			CreateTime: { type: "date", title: "Start Time" },
			CreateUser: { type: "string", title: "Create User" },
			LastEditTime: { type: "date", title: "Last Edit" },
			LastEditUser: { type: "string", title: "Last Edit By" }
		}
	});

});