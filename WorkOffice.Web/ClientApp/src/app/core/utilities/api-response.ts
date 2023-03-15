
export interface  SearchReply<T>{
  totalCount: number;
  result: T;
  errors: string[];
}

export interface  SearchCall<T>{
  PageSize: number;
  From: number;
  Parameter: T;
}

export interface  DeleteReply{
  status: boolean;
  message: string;
}

export interface  CreateResponse{
  id: string;
  status: boolean;
  message: string;
}

export interface  ReportResponse{
  status: boolean;
  downloadUrl: string;
  message: string;
}

export interface  GetResponse<T>{
  status: boolean;
  entity: T;
  message: string;
}
