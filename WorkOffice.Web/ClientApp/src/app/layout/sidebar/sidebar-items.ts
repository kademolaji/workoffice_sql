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
    role: ['All'],
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
    role: ['Admin'],
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
        role: [''],
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
        role: [''],
        submenu: [],
      },
    ],
  },

  // Doctor Modules
  {
    path: '/doctor/dashboard',
    title: 'MENUITEMS.DASHBOARD.LIST.DOCTOR-DASHBOARD',
    iconType: 'material-icons-two-tone',
    icon: 'space_dashboard',
    class: '',
    groupTitle: false,
    badge: '',
    badgeClass: '',
    role: ['Doctor'],
    submenu: [],
  },

  // Patient Modules
  {
    path: '/patient/dashboard',
    title: 'MENUITEMS.DASHBOARD.LIST.PATIENT-DASHBOARD',
    iconType: 'material-icons-two-tone',
    icon: 'space_dashboard',
    class: '',
    groupTitle: false,
    badge: '',
    badgeClass: '',
    role: ['Patient'],
    submenu: [],
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
    role: ['Admin'],
    submenu: [
      {
        path: '/admin/users/all-users',
        title: 'User Account',
        iconType: '',
        icon: '',
        class: 'ml-sub-menu',
        groupTitle: false,
        badge: '',
        badgeClass: '',
        role: [''],
        submenu: [ {
          path: '/admin/users/all-users',
          title: 'All Users',
          iconType: '',
          icon: '',
          class: 'ml-menu2',
          groupTitle: false,
          badge: '',
          badgeClass: '',
          role: [''],
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
          role: [''],
          submenu: [],
        },],
      },
      {
        path: '/admin/company/all-location',
        title: 'Company',
        iconType: '',
        icon: '',
        class: 'ml-sub-menu',
        groupTitle: false,
        badge: '',
        badgeClass: '',
        role: ['Admin'],
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
            role: [''],
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
            role: [''],
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
            role: [''],
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
            role: [''],
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
            role: [''],
            submenu: [],
          }
        ],
      }
    ],
  },
  {
    path: '',
    title: 'Setup',
    iconType: 'material-icons-two-tone',
    icon: 'supervised_user_circle',
    class: 'menu-toggle',
    groupTitle: false,
    badge: '',
    badgeClass: '',
    role: ['Admin'],
    submenu: [
      {
        path: '/admin/activity/all-activity',
        title: 'Activity',
        iconType: '',
        icon: '',
        class: 'ml-menu2',
        groupTitle: false,
        badge: '',
        badgeClass: '',
        role: [''],
        submenu: [],
      },
      {
        path: '/setup/apptype/all-apptype',
        title: 'AppType',
        iconType: '',
        icon: '',
        class: 'ml-menu2',
        groupTitle: false,
        badge: '',
        badgeClass: '',
        role: ['Admin'],
        submenu: [],
      },
      {
            path: '/admin/consultant/all-consultant',
            title: 'Consultant',
            iconType: '',
            icon: '',
            class: 'ml-menu2',
            groupTitle: false,
            badge: '',
            badgeClass: '',
            role: [''],
            submenu: [],
      },
      {
            path: '/admin/hospital/all-hospital',
            title: 'Hospital',
            iconType: '',
            icon: '',
            class: 'ml-menu2',
            groupTitle: false,
            badge: '',
            badgeClass: '',
            role: [''],
            submenu: [],
      }
        ],
  },
  // {
  //   path: '',
  //   title: 'Multi level Menu',
  //   iconType: 'material-icons-two-tone',
  //   icon: 'slideshow',
  //   class: 'menu-toggle',
  //   groupTitle: false,
  //   badge: '',
  //   badgeClass: '',
  //   role: ['Admin'],
  //   submenu: [
  //     {
  //       path: '/multilevel/first1',
  //       title: 'First',
  //       iconType: '',
  //       icon: '',
  //       class: 'ml-menu',
  //       groupTitle: false,
  //       badge: '',
  //       badgeClass: '',
  //       role: [''],
  //       submenu: [],
  //     },
  //     {
  //       path: '/',
  //       title: 'Second',
  //       iconType: '',
  //       icon: '',
  //       class: 'ml-sub-menu',
  //       groupTitle: false,
  //       badge: '',
  //       badgeClass: '',
  //       role: [''],
  //       submenu: [
  //         {
  //           path: '/multilevel/secondlevel/second1',
  //           title: 'Second 1',
  //           iconType: '',
  //           icon: '',
  //           class: 'ml-menu2',
  //           groupTitle: false,
  //           badge: '',
  //           badgeClass: '',
  //           role: [''],
  //           submenu: [],
  //         },
  //         {
  //           path: '/',
  //           title: 'Second 2',
  //           iconType: '',
  //           icon: '',
  //           class: 'ml-sub-menu2',
  //           groupTitle: false,
  //           badge: '',
  //           badgeClass: '',
  //           role: [''],
  //           submenu: [
  //             {
  //               path: '/multilevel/thirdlevel/third1',
  //               title: 'third 1',
  //               iconType: '',
  //               icon: '',
  //               class: 'ml-menu3',
  //               groupTitle: false,
  //               badge: '',
  //               badgeClass: '',
  //               role: [''],
  //               submenu: [],
  //             },
  //           ],
  //         },
  //       ],
  //     },
  //     {
  //       path: '/multilevel/first3',
  //       title: 'Third',
  //       iconType: '',
  //       icon: '',
  //       class: 'ml-menu',
  //       groupTitle: false,
  //       badge: '',
  //       badgeClass: '',
  //       role: [''],
  //       submenu: [],
  //     },
  //   ],
  // },
];
