const securityPoints = {
    address: {
        view: ["ISL.LDS.AdminSpa.Address", "ISL.LDS.AdminSpa.Administrators", "ISL.LDS.AdminSpa.ReadOnly"]
    },
    suppliers: {
        add: ["ISL.LDS.AdminSpa.Suppliers", "ISL.LDS.AdminSpa.Administrators"],
        delete: ["ISL.LDS.AdminSpa.Suppliers", "ISL.LDS.AdminSpa.Administrators"],
        edit: ["ISL.LDS.AdminSpa.Suppliers", "ISL.LDS.AdminSpa.Administrators"],
        view: ["ISL.LDS.AdminSpa.Suppliers", "ISL.LDS.AdminSpa.Administrators"]
    },
    configuration: {
        view: ["ISL.LDS.AdminSpa.Administrators"]
    },
    pds: {
        upload: ["ISL.LDS.AdminSpa.OptOut", "ISL.LDS.AdminSpa.Administrators"],
        view: ["ISL.LDS.AdminSpa.PDS", "ISL.LDS.AdminSpa.Administrators", "ISL.LDS.AdminSpa.ReadOnly"]
    },
    optOut: {
        add: ["ISL.LDS.AdminSpa.OptOut", "ISL.LDS.AdminSpa.Administrators"],
        upload: ["ISL.LDS.AdminSpa.OptOut", "ISL.LDS.AdminSpa.Administrators"],
        readonly: ["ISL.LDS.AdminSpa.ReadOnly"],
        view: ["ISL.LDS.AdminSpa.OptOut", "ISL.LDS.AdminSpa.Administrators", "ISL.LDS.AdminSpa.ReadOnly"]
    },
    ingestionTracking: {
        view: ["ISL.LDS.AdminSpa.IngestionTracking", "ISL.LDS.AdminSpa.Administrators", "ISL.LDS.AdminSpa.ReadOnly"]
    },
    dataSets: {
        view: ["ISL.LDS.AdminSpa.IngestionTracking", "ISL.LDS.AdminSpa.Administrators", "ISL.LDS.AdminSpa.ReadOnly"],
        add: ["ISL.LDS.AdminSpa.Administrators"],
        delete: ["ISL.LDS.AdminSpa.Administrators"],
        edit: ["ISL.LDS.AdminSpa.Administrators"],
    },
    resolvedAddress: {
        view: ["ISL.LDS.AdminSpa.ResolvedAddress", "ISL.LDS.AdminSpa.Administrators", "ISL.LDS.AdminSpa.ReadOnly"],
        add: ["ISL.LDS.AdminSpa.Administrators"],
        edit: ["ISL.LDS.AdminSpa.Administrators"],
    },

    terminologyArtifact: {
        view: ["ISL.LDS.AdminSpa.TerminologyArtifact", "ISL.LDS.AdminSpa.Administrators", "ISL.LDS.AdminSpa.ReadOnly"]
    },
    subscriberAgreement: {
        view: ["ISL.LDS.AdminSpa.Administrators", "ISL.LDS.AdminSpa.ReadOnly"],
        add: ["ISL.LDS.AdminSpa.Administrators"],
        delete: ["ISL.LDS.AdminSpa.Administrators"],
        edit: ["ISL.LDS.AdminSpa.Administrators"],
    },
}

export default securityPoints