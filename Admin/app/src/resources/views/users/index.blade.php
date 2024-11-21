@extends('layouts.app')
@section('title','プレイヤー一覧')
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
    <h3>■ユーザー 一覧■</h3>
    @if(!empty($players))
        {{$players->onEachSide(1)->links()}}
    @endif
    <table class="table table-hover" id="table">
        <tr>
            <th>ID</th>
            <th>名前</th>
            <th>レベル</th>
            <th>経験値</th>
            <th>ライフ</th>
        </tr>
        @foreach($players as $player)
            <tr>
                <td>{{$player['id']}}</td>
                <td>{{$player['name']}}</td>
                <td>{{$player['level']}}</td>
                <td>{{$player['xp']}}</td>
                <td>{{$player['life']}}</td>
            </tr>
        @endforeach
    </table>
@endsection
