export interface IBaseUseCase<T, R = T> {
  execute(...args: any[]): Promise<R>;
}

export interface IQueryUseCase<T, R = T> {
  execute(params?: any): Promise<R>;
}

export interface ICommandUseCase<T, R = T> {
  execute(data: T): Promise<R>;
}