@extends('layouts.app')
@section('title','アイテム一覧')
@section('body')
    @section('userList','nav-link px-2 ')
    @section('userLeg','nav-link px-2')
    @section('player','nav-link px-2')
    @section('item','nav-link px-2 link-secondary')
    @section('personalItem','nav-link px-2')

    <h3>■アイテム一覧■</h3>
    <table class="table table-hover" id="table">
        <tr>
            <th>ID</th>
            <th>名前</th>
            <th>種別</th>
            <th>効果値</th>
            <th>説明</th>
        </tr>
        @foreach($items as $item)
            <tr>
                <td>{{$item['id']}}</td>
                <td>{{$item['item_name']}}</td>
                <td>{{$item['type']}}</td>
                <td>{{$item['effect_num']}}</td>
                <td>{{$item['explanation']}}</td>
            </tr>
        @endforeach
    </table>
@endsection
