@extends('layouts.app')
@section('title','アカウント一覧')
@section('body')
    <h1>■アカウント一覧</h1>
    @if(!empty($accounts))
        {{$accounts->onEachSide(1)->links()}}
    @endif
    <table class="table table-hover" id="table">
        <tr>
            <th>名前</th>
            <th>パスワード</th>
            <th>操作</th>
        </tr>
        @foreach($accounts as $account)
            <tr>
                <td>{{$account['name']}}</td>
                <td>{{$account['password']}}</td>
                <td>
                    @if($account['name'] != $loginName)
                        <form method="post" action="{{route('accounts.confDestroy')}}">
                            @csrf
                            <button type="submit">削除</button>
                            <input type="hidden" name="id" value={{$account['id']}}>
                        </form>
                    @endif
                    <form method="post" action="{{route('accounts.disPassUpdate')}}">
                        @csrf
                        <button type="submit">更新</button>
                        <input type="hidden" name="id" value={{$account['id']}}>
                    </form>
                </td>
            </tr>
        @endforeach
        @if(!empty($error))
            <ul>
                <li class="text-danger">{{$error}}</li>
            </ul>
        @endif
    </table>
@endsection
