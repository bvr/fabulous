
use Path::Class qw(file);

my $file = file("readme.md");

my ($total, $read);
for my $line ($file->slurp) {
    if($line =~ / - \[(x| )\]/) {
        $read += $1 eq 'x';
        $total++;
    }
}

warn sprintf "From $file -- %d (%.2f%%) read out of total %d\n", $read, 100*$read/$total, $total;
warn "![Static Badge](https://img.shields.io/badge/read-$read%20out%20of%20$total-yellow)\n";
warn sprintf "![Static Badge](https://img.shields.io/badge/done-%.2f%%25-green)\n", 100*$read/$total;

