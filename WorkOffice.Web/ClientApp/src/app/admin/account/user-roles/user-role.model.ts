
export interface UserRoleAndActivityModel {
  userRoleAndActivityId: number;
  userRoleDefinition: string;
  activities: UserRoleActivitiesModel[];
}

export interface UserRoleActivitiesModel {
  userRoleActivitiesId: number;
  userRoleDefinitionId: number;
  userRoleDefinition: string;
  userActivityParentId: number;
  userActivityParentName: string;
  userActivityId: number;
  userActivityName: number;
  canEdit: boolean;
  canAdd: boolean;
  canView: boolean;
  canDelete: boolean;
  canApprove: boolean;
  isSelected: boolean;
}

export interface UserActivityParentModel {
  userActivityParentId: number;
  userActivityParentName: string;
  activities: UserActivityModel[];
}

export interface UserActivityModel {
  userActivityId: number;
  userActivityName: string;
  userActivityParentId: number;
}


