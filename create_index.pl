
# simple script to create markdown list of all articles on ericlippert.com

use lib 'lib';
use utf8::all;
use Fabulous;
use Data::Dump;

my $fabulous = Fabulous->new(base_url => 'https://ericlippert.com');

for my $month ($fabulous->get_all_months()) {
    warn "Fetching ", $month->text, "\n";
    print "\n## [", $month->text, "](", $month->{href}, ")\n";
    for my $article ($fabulous->get_articles_for_month($month)) {
        print " - [ ] [", $article->title, "](", $article->href, ")\n";
    }
}
