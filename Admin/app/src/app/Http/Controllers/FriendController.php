<?php

namespace App\Http\Controllers;

use App\Models\followList;
use Illuminate\Http\Request;

class FriendController extends Controller
{
    public function index()
    {
        $follows = followList::all();
        return view('follow/index', ['follows' => $follows]);
    }
}
