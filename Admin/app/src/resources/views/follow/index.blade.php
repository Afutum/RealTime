@extends('layouts.app')
@section('title','フォローリスト')
@section('body')
    @section('userList','nav-link px-2 ')
    @section('userLeg','nav-link px-2')
    @section('mail','nav-link px-2')
    @section('player','nav-link px-2  link-secondary')
    @section('item','nav-link px-2')
    @section('personalItem','nav-link px-2')

    <!--<form method="post" action="{{url('accounts/player')}}">
    <input type="text" name="userName" id="userName">
    <input type="button" class="btn btn-primary" id="button" value="検索">
    <input type="hidden" name="action" value="searchUser">
</form>-->
    <!--<button onclick="location.href='./?action=player'">一覧に戻す</button>-->
    <h3>■フォローリスト■</h3>

    <table class="table table-hover" id="table">
        <tr>
            <th>ID</th>
            <th>名前</th>
            <th>フォローユーザー</th>
            <th>フォローした日</th>
        </tr>
        @foreach($follows as $follow)
            <tr>
                <td>{{$follow['id']}}</td>
                <td>{{$follow->user->name}}</td>
                <td>{{$follow->followUser->name}}</td>
                <td>{{$follow['created_at']}}</td>
            </tr>
        @endforeach
    </table>
@endsection
