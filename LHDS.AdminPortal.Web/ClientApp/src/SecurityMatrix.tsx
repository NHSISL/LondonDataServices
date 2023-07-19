const securityPoints = {
    siteNavigation: {
        view: [""]
    },
    suppliers: {
        add: ["ISL.LDS.AdminSpa.Suppliers", "ISL.LDS.AdminSpa.Administrators"],
        delete: ["ISL.LDS.AdminSpa.Suppliers", "ISL.LDS.AdminSpa.Administrators"],
        edit: ["ISL.LDS.AdminSpa.Suppliers", "ISL.LDS.AdminSpa.Administrators"],
        view: ["ISL.LDS.AdminSpa.Suppliers", "ISL.LDS.AdminSpa.Administrators"]
    },
    configNavigation: {
        view: ["ISL.LDS.AdminSpa.Administrators"]
    },
    pdsNavigation: {
        view: ["ISL.LDS.AdminSpa.PDS", "ISL.LDS.AdminSpa.Administrators", "ISL.LDS.AdminSpa.ReadOnly"]
    },
    pds: {
        upload: ["ISL.LDS.AdminSpa.OptOut", "ISL.LDS.AdminSpa.Administrators"]
    },
    optOutNavigation: {
        view: ["ISL.LDS.AdminSpa.OptOut", "ISL.LDS.AdminSpa.Administrators","ISL.LDS.AdminSpa.ReadOnly"]
    },
    optOut: {
        add: ["ISL.LDS.AdminSpa.OptOut", "ISL.LDS.AdminSpa.Administrators"],
        upload: ["ISL.LDS.AdminSpa.OptOut", "ISL.LDS.AdminSpa.Administrators"],
        readonly: ["ISL.LDS.AdminSpa.ReadOnly"]
    },
    ingestionTrackingNavigation: {
        view: ["ISL.LDS.AdminSpa.IngestionTracking", "ISL.LDS.AdminSpa.Administrators", "ISL.LDS.AdminSpa.ReadOnly"]
    },
}

export default securityPoints