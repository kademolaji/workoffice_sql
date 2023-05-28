import { RouteInfo } from './sidebar.metadata';
export const ROUTES: RouteInfo[] = [
  {
    path: '',
    title: 'MENUITEMS.MAIN.TEXT',
    iconType: '',
    icon: '',
    class: '',
    groupTitle: true,
    badge: '',
    badgeClass: '',
    activity: '',
    submenu: [],
  },

  // Admin Modules
  {
    path: '',
    title: 'MENUITEMS.DASHBOARD.TEXT',
    iconType: 'material-icons-two-tone',
    icon: 'space_dashboard',
    class: 'menu-toggle',
    groupTitle: false,
    badge: '',
    badgeClass: '',
    activity: '',
    submenu: [
      {
        path: '/admin/dashboard/main',
        title: 'MENUITEMS.DASHBOARD.LIST.DASHBOARD1',
        iconType: '',
        icon: '',
        class: 'ml-menu',
        groupTitle: false,
        badge: '',
        badgeClass: '',
        activity: '',
        submenu: [],
      },
      {
        path: '/admin/dashboard/dashboard2',
        title: 'MENUITEMS.DASHBOARD.LIST.DASHBOARD2',
        iconType: '',
        icon: '',
        class: 'ml-menu',
        groupTitle: false,
        badge: '',
        badgeClass: '',
        activity: '',
        submenu: [],
      },
    ],
  },

  // Common Modules

  {
    path: '',
    title: 'Account',
    iconType: 'material-icons-two-tone',
    icon: 'supervised_user_circle',
    class: 'menu-toggle',
    groupTitle: false,
    badge: '',
    badgeClass: '',
    activity: 'account',
    submenu: [
      {
        path: '',
        title: 'User Account',
        iconType: '',
        icon: '',
        class: 'ml-sub-menu',
        groupTitle: false,
        badge: '',
        badgeClass: '',
        activity: 'user accounts',
        submenu: [
          {
            path: '/admin/users/all-users',
            title: 'All Users',
            iconType: '',
            icon: '',
            class: 'ml-menu2',
            groupTitle: false,
            badge: '',
            badgeClass: '',
            activity: 'user accounts',
            submenu: [],
          },
          {
            path: '/admin/users/all-user-roles',
            title: 'User Role',
            iconType: '',
            icon: '',
            class: 'ml-menu2',
            groupTitle: false,
            badge: '',
            badgeClass: '',
            activity: 'user roles',
            submenu: [],
          },
        ],
      },
      {
        path: '',
        title: 'Company',
        iconType: '',
        icon: '',
        class: 'ml-sub-menu',
        groupTitle: false,
        badge: '',
        badgeClass: '',
        activity: 'organization',
        submenu: [
          {
            path: '/admin/company/all-general-information',
            title: 'General Information',
            iconType: '',
            icon: '',
            class: 'ml-menu2',
            groupTitle: false,
            badge: '',
            badgeClass: '',
            activity: 'general information',
            submenu: [],
          },
          {
            path: '/admin/company/all-custom-identity-settings',
            title: 'Custom Identity Settings',
            iconType: '',
            icon: '',
            class: 'ml-menu2',
            groupTitle: false,
            badge: '',
            badgeClass: '',
            activity: 'custom identity settings',
            submenu: [],
          },
          {
            path: '/admin/company/all-location',
            title: 'Location',
            iconType: '',
            icon: '',
            class: 'ml-menu2',
            groupTitle: false,
            badge: '',
            badgeClass: '',
            activity: 'locations',
            submenu: [],
          },
          {
            path: '/admin/company/all-structure-definition',
            title: 'Structure Definition',
            iconType: '',
            icon: '',
            class: 'ml-menu2',
            groupTitle: false,
            badge: '',
            badgeClass: '',
            activity: 'structure definition',
            submenu: [],
          },
          {
            path: '/admin/company/all-company-structure',
            title: 'Company Structure',
            iconType: '',
            icon: '',
            class: 'ml-menu2',
            groupTitle: false,
            badge: '',
            badgeClass: '',
            activity: 'company structures',
            submenu: [],
          },
        ],
      },
    ],
  },
  {
    path: '',
    title: 'NHS',
    iconType: 'material-icons-two-tone',
    icon: 'create_new_folder',
    class: 'menu-toggle',
    groupTitle: false,
    badge: '',
    badgeClass: '',
    activity: 'nhs',
    submenu: [
      {
        path: '/nhs/all-patient',
        title: 'Patient Registration',
        iconType: '',
        icon: '',
        class: 'ml-menu2',
        groupTitle: false,
        badge: '',
        badgeClass: '',
        activity: 'patient information',
        submenu: [],
      },
      {
        path: '/nhs/all-appointment/PartialBooked',
        title: 'Appointment',
        iconType: '',
        icon: '',
        class: 'ml-menu2',
        groupTitle: false,
        badge: '',
        badgeClass: '',
        activity: 'add appointment',
        submenu: [],
      },
      // {
      //   path: '',
      //   title: 'View Appointment Info',
      //   iconType: '',
      //   icon: '',
      //   class: 'ml-sub-menu',
      //   groupTitle: false,
      //   badge: '',
      //   badgeClass: '',
      //   activity: 'view appointment',
      //   submenu: [
      //     {
      //       path: '/nhs/all-appointment/PartialBooked',
      //       title: 'Partial Booked',
      //       iconType: '',
      //       icon: '',
      //       class: 'ml-menu2',
      //       groupTitle: false,
      //       badge: '',
      //       badgeClass: '',
      //       activity: "patient appointment",
      //       submenu: [],
      //     },
      //     {
      //       path: '/nhs/all-appointment/BookedAppointment',
      //       title: 'View Booked Appointment',
      //       iconType: '',
      //       icon: '',
      //       class: 'ml-menu2',
      //       groupTitle: false,
      //       badge: '',
      //       badgeClass: '',
      //       activity: "view booked appointment",
      //       submenu: [],
      //     },
      //     {
      //       path: '/nhs/all-appointment/CancelledAppointment',
      //       title: 'Cancelled Appointment',
      //       iconType: '',
      //       icon: '',
      //       class: 'ml-menu2',
      //       groupTitle: false,
      //       badge: '',
      //       badgeClass: '',
      //       activity: "cancelled appointment",
      //       submenu: [],
      //     },
      //   ],
      // },
      {
        path: '/nhs/all-waitinglist',
        title: 'Waiting List',
        iconType: '',
        icon: '',
        class: 'ml-menu2',
        groupTitle: false,
        badge: '',
        badgeClass: '',
        activity: 'add waitinglist',
        submenu: [],
      },
      {
        path: '/nhs/all-pathway',
        title: 'Pathway',
        iconType: '',
        icon: '',
        class: 'ml-menu2',
        groupTitle: false,
        badge: '',
        badgeClass: '',
        activity: 'add pathway',
        submenu: [],
      },
      {
        path: '/nhs/all-diagnostic',
        title: 'Diagnostic Information',
        iconType: '',
        icon: '',
        class: 'ml-menu2',
        groupTitle: false,
        badge: '',
        badgeClass: '',
        activity: 'diagnostic information',
        submenu: [],
      },
      {
        path: '/nhs/all-referral',
        title: 'Referral Information',
        iconType: '',
        icon: '',
        class: 'ml-menu2',
        groupTitle: false,
        badge: '',
        badgeClass: '',
        activity: 'referral information',
        submenu: [],
      },
      {
        path: '/nhs/validate-now',
        title: 'Validate now',
        iconType: '',
        icon: '',
        class: 'ml-menu2',
        groupTitle: false,
        badge: '',
        badgeClass: '',
        activity: 'validate now',
        submenu: [],
      },
    ],
  },
  {
    path: '',
    title: 'Setup',
    iconType: 'material-icons-two-tone',
    icon: 'settings',
    class: 'menu-toggle',
    groupTitle: false,
    badge: '',
    badgeClass: '',
    activity: 'setup',
    submenu: [
      {
        path: '/setup/nhsactivity/all-nhsactivity',
        title: 'Activity',
        iconType: '',
        icon: '',
        class: 'ml-menu2',
        groupTitle: false,
        badge: '',
        badgeClass: '',
        activity: 'activity',
        submenu: [],
      },
      {
        path: '/setup/apptype/all-apptype',
        title: 'Appointment Type',
        iconType: '',
        icon: '',
        class: 'ml-menu2',
        groupTitle: false,
        badge: '',
        badgeClass: '',
        activity: 'apptype',
        submenu: [],
      },
      {
            path: '/setup/consultant/all-consultant',
            title: 'Consultant',
            iconType: '',
            icon: '',
            class: 'ml-menu2',
            groupTitle: false,
            badge: '',
            badgeClass: '',
            activity: "consultant",
            submenu: [],
      },
      {
            path: '/setup/hospital/all-hospital',
            title: 'Hospital',
            iconType: '',
            icon: '',
            class: 'ml-menu2',
            groupTitle: false,
            badge: '',
            badgeClass: '',
            activity: "hospital",
            submenu: [],
      }
      ,
      {
            path: '/setup/pathwaystatus/all-pathwaystatus',
            title: 'Pathway Status',
            iconType: '',
            icon: '',
            class: 'ml-menu2',
            groupTitle: false,
            badge: '',
            badgeClass: '',
            activity: "pathway status",
            submenu: [],
      }
      ,
      {
            path: '/setup/rtt/all-rtt',
            title: 'RTT',
            iconType: '',
            icon: '',
            class: 'ml-menu2',
            groupTitle: false,
            badge: '',
            badgeClass: '',
            activity: "rtt",
            submenu: [],
      }
      ,
      {
            path: '/setup/specialty/all-specialty',
            title: 'Specialty',
            iconType: '',
            icon: '',
            class: 'ml-menu2',
            groupTitle: false,
            badge: '',
            badgeClass: '',
            activity: "specialty",
            submenu: [],
      }
      ,
      {
            path: '/setup/waitingtype/all-waitingtype',
            title: 'Waiting Type',
            iconType: '',
            icon: '',
            class: 'ml-menu2',
            groupTitle: false,
            badge: '',
            badgeClass: '',
            activity: "waiting type",
            submenu: [],
      }
      ,
      {
            path: '/setup/ward/all-ward',
            title: 'Ward',
            iconType: '',
            icon: '',
            class: 'ml-menu2',
            groupTitle: false,
            badge: '',
            badgeClass: '',
            activity: "ward",
            submenu: [],
      }
        ],
  },
];
